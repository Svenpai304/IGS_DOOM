
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
        private CapsuleCollider collider;
        private PlayerCamera cam;

        private Transform orientation;
        private Vector3 moveDirection;

        private Vector2 mouseInput; 
    
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
            Debug.Log(playerData);
            // Instantiate player objects
            playerObject = playerData.InstantiatePlayer();
            cam = new PlayerCamera(playerObject);
        }

        private void Awake()
        {
            Debug.Log("Player Awake");
            rb = playerObject.GetComponent<Rigidbody>();
            collider = playerObject.GetComponent<CapsuleCollider>();
            
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            
            
            input = new InputActions();

            input.Player.MouseXY.performed += SetMouse;
            input.Player.MouseXY.canceled += SetMouse;
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
            Debug.Log("Player Start");
        }

        private void Update()
        {
            Debug.Log("Player Update");
            cam.UpdateCamera(mouseInput);
        }

        private void FixedUpdate()
        {
            Debug.Log("Player FixedUpdate");
            
        }

        private void SetMouse(InputAction.CallbackContext value)
        {
            mouseInput = value.ReadValue<Vector2>();
            Debug.Log(mouseInput);
        }

        private void MovePlayer(InputAction.CallbackContext value)
        {
        
        }
    }
}
