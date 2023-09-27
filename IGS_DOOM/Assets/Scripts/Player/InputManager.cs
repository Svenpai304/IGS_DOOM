using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager
    {
        /*private Player player;
        private InputActions input;
        public InputManager(Player _player)
        {
            player = _player;
            input = new InputActions();
            GameManager.GlobalAwake += Awake;
            GameManager.GlobalOnEnable += OnEnable;
            GameManager.GlobalOnDisable += OnEnable;
        }
        
        private void Awake()
        {
            input.Player.MouseXY.performed += OnMouseLook;
            input.Player.MouseXY.canceled += OnMouseLook;
            input.Player.Movement.started += OnMovementInput;
            input.Player.Movement.performed += OnMovementInput;
            input.Player.Movement.canceled += OnMovementInput;
            /*input.Player.Crouch.started += OnCrouchInput;
            input.Player.Crouch.canceled += OnCrouchInput;
            input.Player.Walk.started += OnWalkInput;
            input.Player.Walk.canceled += OnWalkInput;
            input.Player.Jump.started += OnJumpInput;
            input.Player.Jump.canceled += OnJumpInput;
            input.Player.Melee.started += OnMeleeInput;
            input.Player.Melee.canceled += OnMeleeInput;
            input.Player.SwitchWeapons.started += OnSwitchWeapon;
            input.Player.SwitchWeapons.canceled += OnSwitchWeapon;
            input.Player.WeaponMods.started += OnWeaponMods;
            input.Player.WeaponMods.canceled += OnWeaponMods;
            input.Player.Fire.started += OnFire;
            input.Player.Fire.canceled += OnFire;
            input.Player.AltFire.started += OnAltFire;
            input.Player.AltFire.canceled += OnAltFire;#1#
        }
 
        private void OnMouseLook(InputAction.CallbackContext obj)
        {
            obj.ReadValue<Vector2>();
        }

        private void OnMovementInput(InputAction.CallbackContext obj)
        {
            obj.ReadValue<Vector2>();
        }
        
        private void OnCrouchInput(InputAction.CallbackContext obj)
        {
            obj.ReadValueAsButton();
        }
        
        private void OnWalkInput(InputAction.CallbackContext obj)
        {
            obj.ReadValueAsButton();
        }
        
        private void OnJumpInput(InputAction.CallbackContext obj)
        {
            obj.ReadValueAsButton();
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }
        
        private void OnDisable()
        {
            input.Player.Disable();
        }*/
    }
}
