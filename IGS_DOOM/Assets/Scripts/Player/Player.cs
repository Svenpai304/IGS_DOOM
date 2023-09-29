using UnityEngine.InputSystem;
using UnityEngine;
using FSM;

namespace Player
{
    public class Player
    {
        private StateController stateController;
        private InputActions input;

        //private PlayerData playerData;
        private GameObject playerObject;
        private Transform pTransform;
        
        public Rigidbody rb;
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
            
            pTransform = playerObject.transform;
            
            var camObj = playerData.CreateCamera();
            cam = new PlayerCamera(playerObject, camObj);
        }

        private void Awake()
        {
            input = new InputActions();
            
            rb = playerObject.GetComponent<Rigidbody>();
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
            isGrounded = Physics.Raycast(pTransform.position, Vector3.down, 2f * 0.5f + .1f, groundLayer);
            
            
            stateController.Update();
            SpeedControl();
            cam.UpdateCamera(mouseInput);

            if (isGrounded) { rb.drag = groundDrag; }
            else { rb.drag = airDrag; }
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
