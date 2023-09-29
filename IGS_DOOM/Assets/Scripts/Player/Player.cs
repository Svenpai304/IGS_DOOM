using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using FSM;
using Unity.VisualScripting;

namespace Player
{

    /*public class PlayerMovementComponent()
    {
        private Rigidbody rb;
        //Movement Variabales


        public PlayerMovementComponent(Rigidbody _rb)
        {
            rb = _rb;
        }

    }*/
    public class Player
    {
        //private PlayerMovementComponent pmc;
        private StateController stateController;
        private InputActions input;

        //private PlayerData playerData;
        private GameObject playerObject;
        private Transform pTransform;
        
        public Rigidbody rb;
        private CapsuleCollider pCollider;
        private PlayerCamera cam;

        private Transform orientation;

        
        private Vector3 moveDirection;

        private Vector2 mouseInput;
        private PlayerData playerData;
        public PlayerData PlayerData => playerData;
        private Vector2 moveInput;
        public bool isGrounded;
        private LayerMask groundLayer;


        private RaycastHit slopeHit;

        private float currentMoveSpeed = 7;



        private float startYScale;
        private float playerRadius;
        

        
        // can initialize from constructor
        private float playerHeight = 2f;


        private float jumpCoolDown;
        private bool readyToJump;
        private bool exitingSlope;
        
        
        // PlayerMovement variables (changable)

        private float airMultiplier = .4f;
        private float jumpForce = 8f;
        private float groundDrag = 5f;
        private float airDrag = 2f;
        private float crouchYScale = .5f;
        private float walkSpeed = 5;
        private float runSpeed = 7;
        private float crouchSpeed = 6;
        private float maxSlopeAngle = 60;
        
        public Player()
        {
            Debug.Log("Player Created");
            GameManager.GlobalAwake += Awake;
            GameManager.GlobalStart += Start;
            GameManager.GlobalUpdate += Update;
            GameManager.GlobalFixedUpdate += FixedUpdate;
            GameManager.GlobalOnEnable += OnEnable;
            GameManager.GlobalOnDisable += OnDisable;

            stateController = new StateController(this);  
            
            // Load player Data from scriptable object
            playerData = Resources.Load<PlayerData>("PlayerData");
            groundLayer = playerData.tGroundLayer;
            
            // Instantiate player objects
            playerObject = playerData.InstantiatePlayer();
            orientation = playerObject.transform.Find("Orientation");
            rb = playerObject.GetComponent<Rigidbody>();
            pCollider = playerObject.GetComponentInChildren<CapsuleCollider>();
            //pmc = new PlayerMovementComponent(rb);
            pTransform = playerObject.transform;
            
            
            var camObj = playerData.CreateCamera();
            cam = new PlayerCamera(playerObject, camObj);
        }

        private void Awake()
        {
            input = new InputActions();
            
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            input.Player.MouseXY.performed += SetMouse;
            input.Player.MouseXY.canceled += SetMouse;
            input.Player.Movement.started += MoveInput;
            input.Player.Movement.performed += MoveInput;
            input.Player.Movement.canceled += MoveInput;
            input.Player.Crouch.started += Crouch;
            input.Player.Crouch.canceled += Crouch;
            input.Player.Jump.started += Jump;
            input.Player.Jump.performed += Jump;

            startYScale = playerObject.transform.localScale.y;
            playerRadius = pCollider.radius;
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            // Grounded check
            // done with CheckCapsule because raycast was very buggy on a little uneven terrain
            Vector3 groundStart = pCollider.bounds.center;
            Vector3 groundEnd = new(pCollider.bounds.center.x, pCollider.bounds.min.y - 0.1f, pCollider.bounds.center.z);
            isGrounded = Physics.CheckCapsule(groundStart, groundEnd, playerRadius, groundLayer);
            Debug.Log(isGrounded);
            
            cam.UpdateCamera(mouseInput);
            
            stateController.Update();

            SpeedControl();

            // MOVE THIS TO A STATE PLS
            LedgeGrab();
            
            if (isGrounded) { rb.drag = groundDrag; }
            else { rb.drag = airDrag; }
        }
        private void LedgeGrab()
        {
            RaycastHit wallHit;
            
            Debug.DrawRay(pTransform.position, orientation.forward);
            // Check for wall
            if (Physics.Raycast(pTransform.position + Vector3.up, orientation.forward, out wallHit, 1f, groundLayer))
            {
                //Debug.Log(wallHit.collider);
                Debug.DrawRay(wallHit.point, Vector3.up, Color.blue);
                RaycastHit capsuleHit;
                float radius = .5f;
                
                // NEED TO FIGURE OUT, CAPSULE CAST TO SEE IF THERE IS ENOUGH SPACE FOR THE ENTIRE PLAYER
                
                //Vector3 capPos1 = wallHit.point + (orientation.forward * radius) + (Vector3.up * (.5f * playerHeight));
                //Vector3 capPos2 = wallHit.point + (orientation.forward * radius) + (Vector3.down * (.5f * playerHeight));
                //Debug.DrawLine(capPos1, capPos2, Color.green);
                Vector3 downRay = wallHit.point + (orientation.forward * radius) + (Vector3.up * (.5f * playerHeight));
                
                Debug.DrawRay(downRay, Vector3.down, Color.green);
                if (Physics.Raycast(downRay,Vector3.down, out capsuleHit, playerHeight, groundLayer))  //Physics.Raycast(capPos1, capPos2, radius, orientation.forward, out capsuleHit))
                {
                    // MAKE VARIABLE IN THE FUTURE FOR VAULTSPEED
                    GameManager.Instance.StartCoroutine(LerpToVaultPos(capsuleHit.point, .175f));
                    
                 
                    //Debug.Log(capsuleHit.collider);
                }
            }
            else
            {
                Debug.Log("No Wall");
            }
            
            
            // If there is a wall, raycast for ledgedetection
            // if there is a ledge, and enough room to stand onto the ledge
            // lerp te player to the ledgegrabbed position

        }

        private IEnumerator LerpToVaultPos(Vector3 _targetPos, float duration)
        {
            float time = 0;
            Vector3 startPos = pTransform.position;
            Vector3 targetPos = new (_targetPos.x, _targetPos.y + playerHeight/2, _targetPos.z);
            while (time < duration)
            {
                rb.MovePosition(Vector3.Lerp(startPos, targetPos, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            rb.MovePosition(targetPos);
        }

        private void FixedUpdate()
        {
            stateController.FixedUpdate();
        }

        public void PlayerMove()
        {
            moveDirection = playerObject.transform.forward * moveInput.y + playerObject.transform.right * moveInput.x;

            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMovementDirection() * (currentMoveSpeed * 20f), ForceMode.Force);

                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80, ForceMode.Force);
                }
            }
            
            if (isGrounded)
                rb.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f), ForceMode.Force);
            else if (!isGrounded)
                rb.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f * airMultiplier), ForceMode.Force);

            rb.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > currentMoveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * currentMoveSpeed;
                }
            }
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if (flatVel.magnitude > currentMoveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }

        private void SetMouse(InputAction.CallbackContext value)
        {
            mouseInput = value.ReadValue<Vector2>();
        }

        private void MoveInput(InputAction.CallbackContext value)
        {
            moveInput = value.ReadValue<Vector2>();
        }

        private void Jump(InputAction.CallbackContext value)
        {
            readyToJump = false;
            if (isGrounded)
            {
                PerformJump();
            }
            
            ResetJump();
        }

        private void Crouch(InputAction.CallbackContext value)
        {
            if (value.ReadValueAsButton())
            {
                pTransform.localScale = new Vector3(pTransform.localScale.x, crouchYScale, pTransform.localScale.z);
            
                rb.AddForce(Vector3.down, ForceMode.Impulse);
            }
            else
            {
                pTransform.localScale = new Vector3(pTransform.localScale.x, startYScale, pTransform.localScale.z);
            }
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(playerObject.transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + .3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMovementDirection()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }

        private void PerformJump()
        {
            exitingSlope = true;
            
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            if (isGrounded)
            {
                readyToJump = true;
                
                exitingSlope = false;
            }
        }
    }
}
