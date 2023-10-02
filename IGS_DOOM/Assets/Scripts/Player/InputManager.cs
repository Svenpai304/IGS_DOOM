using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : InputActions.IPlayerActions
    {
        private InputActions input;

        public Action<bool> OnJumpPressed;
        public Action OnWalkPressed;
        public Action OnCrouchPressed;
        public Action OnFirePressed;
        public Action OnSwitchWeaponsPressed;
        public Action OnWeaponModsPressed;
        public Action OnMeleePressed;
        public Action OnAltFirePressed;
        public Action<Vector2> MovementInput;
        public Action<Vector2> MouseInput;
        
        public InputManager()
        {
            input = new InputActions();
            
            GameManager.GlobalOnEnable += OnEnable;
            GameManager.GlobalOnDisable += OnDisable;
        }

        private void OnEnable()
        {
            Debug.Log("Input ENabled");
            input.Player.SetCallbacks(this);
            input.Player.Enable();
        }
        
        private void OnDisable()
        {
            input.Player.Disable();
        }

        public void OnMouseXY(InputAction.CallbackContext context)
        {
            // Performed
            MouseInput?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            // Hold / Tap
            MovementInput?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // Tap
            if (!context.performed) { return; }
            OnJumpPressed?.Invoke(context.ReadValueAsButton());
        }

        public void OnWalk(InputAction.CallbackContext context)
        {
            // Hold
            OnWalkPressed?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            // Tap
            if (!context.performed) { return; }
            OnCrouchPressed?.Invoke();
        }

        public void OnMelee(InputAction.CallbackContext context)
        {
            // tap
            if (!context.performed) { return; }
            OnMeleePressed?.Invoke();
        }

        public void OnSwitchWeapons(InputAction.CallbackContext context)
        {
            // tap
            if (!context.performed) { return; }
            OnSwitchWeaponsPressed?.Invoke();
        }

        public void OnWeaponMods(InputAction.CallbackContext context)
        {
            // tap
            if (!context.performed) { return; }
            OnWeaponModsPressed?.Invoke();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            // need to figure out
            if (!context.performed) { return; }
            OnFirePressed?.Invoke();
        }

        public void OnAltFire(InputAction.CallbackContext context)
        {
            // need to figure out
            if (!context.performed) { return; }
            OnAltFirePressed?.Invoke();
        }
    }
}
