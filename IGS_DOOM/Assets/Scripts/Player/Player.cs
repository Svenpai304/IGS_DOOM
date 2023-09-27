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

        private float moveSpeed = 7f;
        private float groundDrag = 5f;
        private float airDrag = 2f;
        private float playerHeight = 2f;

        private float jumpForce = 8f;
        private float jumpCoolDown;
        private float airMultiplier = .4f;
        private bool readyToJump;

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
            input.Player.Jump.started += Jump;
            input.Player.Jump.performed += Jump;
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

            if (grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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

        private void PerformJump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            if (grounded)
            {
                readyToJump = true;
            }
        }
    }
}
