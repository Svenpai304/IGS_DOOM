using System;
using UnityEngine;

namespace Player
{
    public class PlayerCamera
    {
        private Camera camera;
        private GameObject player;

        private Vector2 sens = new (200, 200);
        private Vector2 rotation;

        public PlayerCamera(GameObject _player)
        {
            player = _player;
            camera = player.GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    
        public void UpdateCamera(Vector2 _mouseInput)
        {
            // get input
            Vector2 mouseInput = _mouseInput * Time.deltaTime * sens;
            
            rotation.y += mouseInput.x;
            rotation.x -= mouseInput.y;
            rotation.x = Math.Clamp(rotation.x, -90f, 90f);
            
            // rotate cam and player
            camera.transform.localRotation = Quaternion.Euler(rotation.x, 0 ,0);
            player.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
    }
}
