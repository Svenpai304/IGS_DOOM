using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using FSM;
using Unity.VisualScripting;

namespace Player
{ 
    public class Player
    {
        private StateController stateController;
        private InputActions input;
        private CharacterMovementComponent cmc;

        //private PlayerData playerData;
        private GameObject playerObject;
        private Transform pTransform;
        private CapsuleCollider pCollider;
        private PlayerCamera cam;


        private Vector2 mouseInput;
        private PlayerData playerData;
        public PlayerData PlayerData => playerData;
        private MovementVariables pMoveData;
        
        private Vector2 moveInput;

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
            playerObject = playerData.InstantiatePlayer();
            
            pMoveData = playerData.PMoveData;
            pMoveData.RB = playerObject.GetComponent<Rigidbody>();
            pMoveData.Orientation = playerObject.transform.Find("Orientation");
            pMoveData.Collider = playerObject.GetComponentInChildren<CapsuleCollider>();
            
            // Instantiate player objects
            groundLayer = playerData.GroundLayer;

            //inputManager = new InputManager();
            
            //cmc = new CharacterMovementComponent(rb);
            pTransform = playerObject.transform;
            
            stateController = new StateController(pMoveData);  
            
            var camObj = playerData.CreateCamera();
            cam = new PlayerCamera(playerObject, camObj);
        }

        private void Awake()
        {
            input = new InputActions();
            // Subscribe to the Input Events
            /*inputManager.OnJumpPressed += JumpInput;
            inputManager.OnCrouchPressed += CrouchInput;
            inputManager.OnWalkPressed += WalkInput;
            inputManager.MovementInput += MoveInput;
            inputManager.MouseInput += MouseInput;*/

            input.Player.MouseXY.started += MouseInput;
            input.Player.MouseXY.performed += MouseInput;
            input.Player.MouseXY.canceled += MouseInput;
            input.Player.Movement.started += MoveInput;
            input.Player.Movement.performed += MoveInput;
            input.Player.Movement.canceled += MoveInput;
            input.Player.Jump.started += JumpInput;
            input.Player.Jump.canceled += JumpInput;
            input.Player.Crouch.started += CrouchInput;
            input.Player.Crouch.canceled += CrouchInput;
            input.Player.Walk.started += WalkInput;
            input.Player.Walk.performed += WalkInput;
            input.Player.Walk.canceled += WalkInput;


            pMoveData.RB.freezeRotation = true;
            pMoveData.RB.interpolation = RigidbodyInterpolation.Interpolate;
            pMoveData.RB.collisionDetectionMode = CollisionDetectionMode.Continuous;

            InitPlayerVariables();
        }

        private void InitPlayerVariables()
        {
            playerRadius = pMoveData.Collider.radius;
            playerHeight = pMoveData.Collider.height;
            playerHalfHeight = playerHeight / 2;
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

        private void WalkInput(InputAction.CallbackContext callbackContext)
        {
            isWalking = callbackContext.ReadValueAsButton();
        }

        private void MouseInput(InputAction.CallbackContext callbackContext)
        {
            mouseInput = callbackContext.ReadValue<Vector2>();
        }

        private void SetMouse(InputAction.CallbackContext value)
        {
            mouseInput = value.ReadValue<Vector2>();
        }

        private void MoveInput(InputAction.CallbackContext callbackContext)
        {
            moveInput = callbackContext.ReadValue<Vector2>();
        }
        
        private void JumpInput(InputAction.CallbackContext callbackContext)
        {
            if (coyoteTimeCounter > 0f && callbackContext.ReadValueAsButton())
            {
                if (isGrounded || pMoveData.CanDoubleJump)
                {
                    readyToJump = false;
                    PerformJump();

                    pMoveData.CanDoubleJump = !pMoveData.CanDoubleJump;
                    
                    // FIND A WAY TO RESET THE JUMP VARIABLES. WITH THAT ALSO THE DOUBLE JUMP AND COYOTETIME
                    // SLOPE JUMPING WILL BE ENABLED THIS WAY
                    // THIS DOESNT WORK BECAUSE THE SYSTEM ISNT RESETTING THE ISEXETING SLOPE VAR
                    // PROBS BEST WAY IS VIA THE STATE MACHINE
                    //ResetJump();
                }
            }

            if (!callbackContext.ReadValueAsButton())
            {
                // THIS NEEDS TO MOVE I THINK :)
                coyoteTimeCounter = 0f;
            }
        }

        private void CrouchInput(InputAction.CallbackContext callbackContext)
        {
            if (true)
            {
                // Crouch()
                pTransform.localScale = new(pTransform.localScale.x, crouchYScale, pTransform.localScale.z);

                // LERP THE SCALE FOR SMOOTHER CROUCH
                pMoveData.RB.AddForce(Vector3.down, ForceMode.Impulse);

                GameManager.Instance.StartCoroutine(HandleCrouch(crouchYScale, .2f));
            }
            else
            {
                // UnCrouch();
                pTransform.localScale = new(pTransform.localScale.x, startYScale, pTransform.localScale.z);
                GameManager.Instance.StartCoroutine(HandleCrouch(startYScale, .2f));
            }
        }
        
        private void Update()
        {
            // Grounded check
            // done with CheckCapsule because raycast was very buggy on a little uneven terrain
            var bounds = pMoveData.Collider.bounds;
            Vector3 groundStart = bounds.center;
            Vector3 groundEnd = new (bounds.center.x, bounds.min.y - .1f, bounds.center.z);
            isGrounded = Physics.CheckCapsule(groundStart, groundEnd, playerRadius, groundLayer);
            Debug.Log(isGrounded);
            
            cam.UpdateCamera(mouseInput);
            
            // clamp the speed before applying it to the player
            SpeedControl();
            
            // Update the state controller
            stateController.Update();

            if (isGrounded) 
            { 
                Debug.Log("is Grounded");
                pMoveData.RB.drag = pMoveData.GroundDrag;
                //exitingSlope = false;
                readyToJump = true;
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
                pMoveData.RB.drag = pMoveData.AirDrag;
            }
        }
    }
}
