using System;
using UnityEngine.InputSystem;
using UnityEngine;
using Player.Pickups;
using FSM;
using Weapons;

//-----------------------------------------------------------------------------------
// Player class is mainly used for handling the Input and a commonplace for variables
// All the logic that the player needs is attached to this object/script
//-----------------------------------------------------------------------------------

namespace Player
{
    public struct InputData
    {
        public bool         IsWalking;
        public bool         IsCrouching;
        public InputAction  Jump;
        public Vector2      MoveInput;
    }

    public class Player : HSA_Component, IWeaponHolder, IStateData, IObserver
    {
        private StateController    stateController;
        private InputActions       input;
        private InputData          inputData;
        private CMC                cmc;
        private PickupManager      pickupManager;
        
        private GameObject         playerObject;
        private PlayerCamera       cam;
        
        private Vector2            mouseInput;
        private PlayerData         playerData;
        private WeaponCarrier      weapons;
        private MoveVar            pMoveData;
        
        public float               Health;
        public float               Shield;
        public float               Ammo;
        
        public  Transform          CamTransform { get; set; }
        public  Transform          WeaponTransform { get; set; }
        public  ScratchPad         SharedData { get; }

        public Player()
        {
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
            
            input = new InputActions();

            // Movement input
            input.Player.MouseXY.started += MouseInput;
            input.Player.MouseXY.performed += MouseInput;
            input.Player.MouseXY.canceled += MouseInput;
            input.Player.Movement.started += MoveInput;
            input.Player.Movement.performed += MoveInput;
            input.Player.Movement.canceled += MoveInput;
            input.Player.Jump.started += JumpInput;
            inputData.Jump = input.Player.Jump;
            input.Player.Crouch.started += CrouchInput;
            input.Player.Walk.started += WalkInput;
            input.Player.Walk.canceled += WalkInput;

            // Fire and damage abilities input
            input.Player.Fire.started += FireInput;
            input.Player.Fire.canceled += FireInput;
            input.Player.AltFire.started += AltFireInput;
            input.Player.AltFire.canceled += AltFireInput;
            input.Player.SwitchWeapons.started += SwitchWeaponsInput;
            input.Player.WeaponMods.started += WeaponModsInput;
            input.Player.Melee.started += MeleeInput;
            input.Enable();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void Update()
        {
            SharedData.Set("input", inputData);
            SharedData.Set("Movement", pMoveData);

            cam.UpdateCamera(mouseInput);
            stateController.Update();
        }

        private void FixedUpdate()
        {
            stateController.FixedUpdate();
        }

        public void OnNotify(Pickup _pickup)
        {
            switch (_pickup.Type)
            {
                case PickupType.Health:
                    Debug.Log(_pickup.Amount + " Health picked up");
                    break;
                case PickupType.Shield:
                    Debug.Log(_pickup.Amount + " Shield picked up");
                    break;
                case PickupType.Ammo:
                    Debug.Log(_pickup.Amount + " Ammo picked up");
                    break;
            }
        }

        #region Input 

            private void MouseInput(InputAction.CallbackContext callbackContext)
            {
                mouseInput = callbackContext.ReadValue<Vector2>();
            }

            private void MoveInput(InputAction.CallbackContext callbackContext)
            { 
                inputData.MoveInput = callbackContext.ReadValue<Vector2>();
            }

            private void WalkInput(InputAction.CallbackContext callbackContext)
            {
                inputData.IsWalking = callbackContext.ReadValueAsButton();
            }

            private void CrouchInput(InputAction.CallbackContext callbackContext)
            {
                if (callbackContext.ReadValueAsButton())
                {
                    inputData.IsCrouching = !inputData.IsCrouching;
                }
            }

            private void JumpInput(InputAction.CallbackContext callbackContext)
            {
                // Jump is an event so the actual jumping will be read from the states
                // This function just ensures that when you jump whilst in the crouching state you stand up
                if (callbackContext.ReadValueAsButton() && inputData.IsCrouching)
                {
                    inputData.IsCrouching = false;
                }
            }

            private bool canSwitchForward = true;
            private void SwitchWeaponsInput(InputAction.CallbackContext callbackContext)
            {
                // flip flop
                if (callbackContext.ReadValueAsButton() && canSwitchForward)
                {
                    canSwitchForward = false;
                    weapons.SwitchWeaponForward();
                }
                else
                {
                    canSwitchForward = true;
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
                if (callbackContext.ReadValueAsButton())
                {
                    weapons.CurrentWeapon.FirePressed();
                }
                else
                {
                    weapons.CurrentWeapon.FireReleased();
                }
            }

            private void WeaponModsInput(InputAction.CallbackContext callbackContext)
            {
                if (callbackContext.ReadValueAsButton())
                {
                    weapons.CurrentWeapon.SwitchMod();
                }
            }
        
            private void MeleeInput(InputAction.CallbackContext callbackContext)
            {
                RaycastHit hit;
                if (callbackContext.ReadValueAsButton())
                {
                    if (Physics.BoxCast(CamTransform.position, new Vector3(.125f, .125f, .125f), CamTransform.forward, out hit, CamTransform.rotation, 10, LayerMask.GetMask("Damageable")))
                    {
                        if (hit.distance < pMoveData.MeleeDistance)
                        {
                            if (EnemyManager.EnemyDict.ContainsKey(hit.collider.name))
                            {
                                EnemyManager.EnemyDict[hit.collider.name].TakeDamage(pMoveData.MeleeDmg);
                            }
                        }
                    }
                }
            }
        #endregion
    }
}
