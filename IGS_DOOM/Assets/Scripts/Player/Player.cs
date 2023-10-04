using UnityEngine.InputSystem;
using UnityEngine;
using FSM;
using Weapons;

namespace Player
{
    public struct InputData
    {
        public bool IsWalking;
        public bool IsRunning;
        public bool IsCrouching;
        public InputAction Jump;
        public Vector2 MoveInput;
    }
    
    public class Player : IWeaponHolder, IStateData
    {
        public ScratchPad SharedData { get; }
        private StateController stateController;
        private InputActions input;
        private CMC cmc;

        private InputData inputData;
        
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
        private MoveVar pMoveData;
        
        private Vector2 moveInput;

        public Player()
        {
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
            var camObj = playerData.CreateCamera();
            cam = new PlayerCamera(playerObject, camObj);
            CamTransform = camObj.transform;
            WeaponTransform = camObj.transform.Find("WeaponHolder");
            weapons = new WeaponCarrier(this);
            cmc = new CMC(playerData);

            SharedData = new ScratchPad();
            SharedData.Set("cmc", cmc);
            stateController = new StateController(this);  
            
            pMoveData.RB.freezeRotation = true;
            pMoveData.RB.interpolation = RigidbodyInterpolation.Interpolate;
            pMoveData.RB.collisionDetectionMode = CollisionDetectionMode.Continuous;
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
            inputData.Jump = input.Player.Jump;
            input.Player.Crouch.started += CrouchInput;
            input.Player.Walk.started += WalkInput;
            input.Player.Walk.canceled += WalkInput;

            input.Player.Fire.started += FireInput;
            input.Player.Fire.canceled += FireInput;
            input.Player.AltFire.started += AltFireInput;
            input.Player.AltFire.canceled += AltFireInput;
            input.Player.SwitchWeapons.started += SwitchWeaponsInput;
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }
        
        private void Update()
        {
            cam.UpdateCamera(mouseInput);
            SharedData.Set("input", inputData);
            SharedData.Set("Movement", pMoveData);
            stateController.Update();
        }

        private void FixedUpdate()
        {
            stateController.FixedUpdate();
        }
        
        #region Input

            private void WalkInput(InputAction.CallbackContext callbackContext)
            {
                inputData.IsWalking = callbackContext.ReadValueAsButton();
            }

            private void MouseInput(InputAction.CallbackContext callbackContext)
            {
                mouseInput = callbackContext.ReadValue<Vector2>();
            }

            private void MoveInput(InputAction.CallbackContext callbackContext)
            {
                inputData.MoveInput = callbackContext.ReadValue<Vector2>();
            }

            private void CrouchInput(InputAction.CallbackContext callbackContext)
            {
                if (callbackContext.ReadValueAsButton())
                {
                    inputData.IsCrouching = !inputData.IsCrouching;
                }
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

        #endregion
    }
}
