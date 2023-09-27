
using Unity.VisualScripting;

namespace Player
{
using UnityEngine.InputSystem;
using UnityEngine;
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
        private Vector2 moveInput;
        private bool grounded;
        private LayerMask groundLayer;

        private float moveSpeed = 4;
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
            var playerData = Resources.Load<PlayerData>("PlayerData");
            groundLayer = playerData.GroundLayer();
            Debug.Log(playerData.testString);
            // Instantiate player objects
            playerObject = playerData.InstantiatePlayer();
            cam = new PlayerCamera(playerObject);
        }

        private void Awake()
        {
            rb = playerObject.GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            input = new InputActions();

            input.Player.MouseXY.performed += SetMouse;
            input.Player.MouseXY.canceled += SetMouse;
            input.Player.Movement.performed += MoveInput;
            input.Player.Movement.canceled += MoveInput;
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
            grounded = Physics.Raycast(position, Vector3.down, 2f * 0.5f + .1f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(position, Vector3.down * (2f * 0.5f + 0.1f), Color.green);
            Debug.Log(grounded);
            cam.UpdateCamera(mouseInput);
        }

        private void FixedUpdate()
        {
            moveDirection = playerObject.transform.forward * moveInput.y + playerObject.transform.right * moveInput.x;
            
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        private void SetMouse(InputAction.CallbackContext value)
        {
            mouseInput = value.ReadValue<Vector2>();
        }

        private void MoveInput(InputAction.CallbackContext value)
        {
            moveInput = value.ReadValue<Vector2>();
        }
    }
}
