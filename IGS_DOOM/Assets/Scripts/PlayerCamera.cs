using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera
{
    private GameObject cameraObject;

    private GameObject player;
    private Vector2 sens;
    private Vector2 rotation;
    private Transform playerTransform;
    
    public PlayerCamera(GameObject _player)
    {
        player = _player;
        cameraObject = GameObject.Find("MainCamera");;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void UpdateCamera(Vector2 _mouseInput, Transform _playerTransform)
    {
        Vector2 mouseInput = _mouseInput;
        rotation.y += mouseInput.x;
        
        rotation.x -= mouseInput.y;
        rotation.x = Math.Clamp(rotation.x, -90f, 90f);
        
        cameraObject.transform.rotation = Quaternion.Euler(rotation);
        _playerTransform.rotation = Quaternion.Euler(0, rotation.y, 0);

        //return _playerTransform;
    }
}
