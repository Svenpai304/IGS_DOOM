using System;
using UnityEngine;

namespace Player
{
    public class PlayerCamera
    {
        private GameObject camera;
        private GameObject player;
        private Transform camHolder;

        private Vector2 sens = new (100, 100);
        private Vector2 rotation;

        public PlayerCamera(GameObject _player, GameObject _camObj)
        {
            player = _player;
            camera = _camObj;
            camHolder = player.transform.Find("CamHolder");
            
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
            camera.transform.localRotation = Quaternion.Euler(rotation);
            player.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
            
            camera.transform.position = camHolder.transform.position;
        }
    }
}
