using UnityEngine.InputSystem;
using UnityEngine;

namespace Player
{
    public class Player
    {
        private InputActions input;

        //private PlayerData playerData;
        private GameObject playerObject;
        private Rigidbody rb;
        private PlayerCamera cam;

        private Transform orientation;

        
        private Vector3 moveDirection;

        private Vector2 mouseInput;
        private PlayerData playerData;
        public PlayerData PlayerData => playerData;
        private Vector2 moveInput;
        private bool grounded;
        private LayerMask groundLayer;

        private float maxSlopeAngle = 50;
        private RaycastHit slopeHit;

        private float currentMoveSpeed = 7;
        private float walkSpeed = 5;
        private float runSpeed = 7;
        private float crouchSpeed = 6;

        private float crouchYScale = .5f;
        private float startYScale;
        
        private float groundDrag = 5f;
        private float airDrag = 2f;
        
        private float playerHeight = 2f;

        private float jumpForce = 8f;
        private float jumpCoolDown;
        private float airMultiplier = .4f;
        private bool readyToJump;
        private bool exetingSlope;

        public Player()
        {
            Debug.Log("Player Created");
            GameManager.GlobalAwake += Awake;
            GameManager.GlobalStart += Start;
            GameManager.GlobalUpdate += Update;
            GameManager.GlobalFixedUpdate += FixedUpdate;
            GameManager.GlobalOnEnable += OnEnable;
            GameManager.GlobalOnDisable += OnDisable;
            
            // Load player Data from scriptable object
            playerData = Resources.Load<PlayerData>("PlayerData");
            groundLayer = playerData.tGroundLayer;
            
            // Instantiate player objects
            playerObject = playerData.InstantiatePlayer();
            orientation = playerObject.transform.Find("Orientation");
            
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
            var position = playerObject.transform.position;
            grounded = Physics.Raycast(position, Vector3.down, 2f * 0.5f + .1f, groundLayer);
            SpeedControl();
            
            Debug.Log(OnSlope());

            if (grounded) { rb.drag = groundDrag; }
            else { rb.drag = airDrag; }
            
            
            cam.UpdateCamera(mouseInput);
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void PlayerMove()
        {
            moveDirection = playerObject.transform.forward * moveInput.y + playerObject.transform.right * moveInput.x;

            if (OnSlope())
            {
                rb.AddForce(GetSlopeMovementDirection() * (currentMoveSpeed * 20f), ForceMode.Force);

                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80, ForceMode.Force);
                }
            }
            
            if (grounded)
                rb.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f), ForceMode.Force);
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f * airMultiplier), ForceMode.Force);

            rb.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            if (OnSlope() && !exetingSlope)
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
            if (grounded)
            {
                PerformJump();
            }
            
            ResetJump();
        }

        private void Crouch(InputAction.CallbackContext value)
        {
            if (value.ReadValueAsButton())
            {
                            
                var scale = playerObject.transform.localScale;
                scale = new Vector3(scale.x, crouchYScale, scale.z);
                playerObject.transform.localScale = scale;
            
                rb.AddForce(Vector3.down, ForceMode.Impulse);
            }
            else
            {
                var scale = playerObject.transform.localScale;
                scale = new Vector3(scale.x, startYScale, scale.z);
                playerObject.transform.localScale = scale;
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
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }

        private void PerformJump()
        {
            exetingSlope = true;
            
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            if (grounded)
            {
                readyToJump = true;
                
                exetingSlope = false;
            }
        }
    }
}
