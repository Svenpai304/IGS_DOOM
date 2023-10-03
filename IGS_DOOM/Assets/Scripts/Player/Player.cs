using UnityEngine.InputSystem;
using UnityEngine;
using FSM;
using Unity.VisualScripting;
using Weapons;

namespace Player
{ 
    public class Player : IWeaponHolder
    {
        private StateController stateController;
        private InputActions input;
        private CharacterMovementComponent cmc;
        
        //private PlayerData playerData;
        public Transform WeaponTransform { get; set; }
        public Transform CamTransform { get; set; }
        private GameObject playerObject;
        private Transform pTransform;
        private CapsuleCollider pCollider;
        private PlayerCamera cam;
        private bool isCrouchingPressed;

        private Vector2 mouseInput;
        private PlayerData playerData;
        private WeaponCarrier weapons;
        public PlayerData PlayerData => playerData;
        private MovementVariables pMoveData;
        
        private Vector2 moveInput;

        public Player()
        {
            Debug.Log("Player Created");
            GameManager.GlobalAwake += Awake;
            GameManager.GlobalUpdate += Update;
            GameManager.GlobalFixedUpdate += FixedUpdate;
            GameManager.GlobalOnEnable += OnEnable;
            GameManager.GlobalOnDisable += OnDisable;

            // Load player Data from scriptable object
            playerData = Resources.Load<PlayerData>("PlayerData");
            playerObject = playerData.InstantiatePlayer();
            pMoveData = playerData.PMoveData;
            pMoveData.RB = playerObject.GetComponent<Rigidbody>();
            pMoveData.Collider = playerObject.GetComponentInChildren<CapsuleCollider>();
            pMoveData.Orientation = playerObject.transform.Find("Orientation");
            pMoveData.StepUpMin = playerObject.transform.Find("stepUpMin");
            pMoveData.StepUpMax = playerObject.transform.Find("stepUpMax");
            pMoveData.pTransform = playerObject.transform;
            
            // Instantiate player objects

            cmc = new CharacterMovementComponent(playerData);
            
            stateController = new StateController(cmc);  
            
            var camObj = playerData.CreateCamera();
            cam = new PlayerCamera(playerObject, camObj);
            CamTransform = camObj.transform;
            WeaponTransform = camObj.transform.Find("WeaponHolder");
            weapons = new WeaponCarrier(this);
        }

        private void Awake()
        {
            input = new InputActions();

            input.Player.MouseXY.started += MouseInput;
            input.Player.MouseXY.performed += MouseInput;
            input.Player.MouseXY.canceled += MouseInput;
            input.Player.Movement.started += MoveInput;
            input.Player.Movement.performed += MoveInput;
            input.Player.Movement.canceled += MoveInput;
            input.Player.Jump.started += JumpInput;
            input.Player.Crouch.started += CrouchInput;
            input.Player.Walk.started += WalkInput;
            input.Player.Walk.canceled += WalkInput;

            input.Player.Fire.started += FireInput;
            input.Player.Fire.canceled += FireInput;
            input.Player.AltFire.started += AltFireInput;
            input.Player.AltFire.canceled += AltFireInput;
            input.Player.SwitchWeapons.started += SwitchWeaponsInput;
            
            pMoveData.RB.freezeRotation = true;
            pMoveData.RB.interpolation = RigidbodyInterpolation.Interpolate;
            pMoveData.RB.collisionDetectionMode = CollisionDetectionMode.Continuous;

        }

        private bool isPreviousWeapon = true;
        private void SwitchWeaponsInput(InputAction.CallbackContext callbackContext)
        {
            // flip flop
            if (callbackContext.ReadValueAsButton() && isPreviousWeapon)
            {
                isPreviousWeapon = false;
                weapons.SwitchToNextWeapon();
            }
            else
            {
                isPreviousWeapon = true;
                weapons.SwitchToPreviousWeapon();
            }
        }

        private void AltFireInput(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                weapons.CurrentWeapon.AltFirePressed();
            }
            else
            {
                weapons.CurrentWeapon.AltFireReleased();
            }
        }

        private void FireInput(InputAction.CallbackContext callbackContext)
        {
            RaycastHit hit;
            if (callbackContext.ReadValueAsButton())
            {

                if (Physics.Raycast(CamTransform.position,CamTransform.forward, out hit,100, LayerMask.GetMask("Damageable")))
                {
                    Debug.Log(hit.collider.name);
                }
                weapons.CurrentWeapon.FirePressed();
            }
            else
            {
                weapons.CurrentWeapon.FireReleased();
            }
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void WalkInput(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                pMoveData.IsWalking = true;
                stateController.ChangeState(stateController.WalkState);
            }
            else
            {
                pMoveData.IsWalking = false;
                stateController.ChangeState(stateController.RunState);
            }
        }

        private void MouseInput(InputAction.CallbackContext callbackContext)
        {
            mouseInput = callbackContext.ReadValue<Vector2>();
        }

        private void MoveInput(InputAction.CallbackContext callbackContext)
        {
            moveInput = callbackContext.ReadValue<Vector2>();
        }
        
        private void JumpInput(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton() && !pMoveData.IsCrouching)
            {
                // You dont want to limit the jump action because of the double jump
                stateController.ChangeState(stateController.JumpState);
            }
            else
            {
                pMoveData.IsCrouching = false;
                stateController.ChangeState(stateController.RunState);
            }
        }
        
        private void CrouchInput(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton() && !pMoveData.IsCrouching)
            {
                pMoveData.IsCrouching = true;
                stateController.ChangeState(stateController.CrouchState);
            }
            else
            {
                pMoveData.IsCrouching = false;
                stateController.ChangeState(stateController.RunState);
            }
        }
        
        private void Update()
        {
            cam.UpdateCamera(mouseInput);
        }

        private void FixedUpdate()
        {
            if (!pMoveData.IsGrounded)
            {
                stateController.ChangeState(stateController.InAirState);
            }
            cmc.PlayerMove(moveInput);
        }
    }
}
