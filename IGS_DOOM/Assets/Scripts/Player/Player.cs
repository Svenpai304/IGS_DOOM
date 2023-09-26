using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


// monobehaviour for dev purposes only
public class Player : MonoBehaviour
{
    private InputActions input;

    private GameObject playerObject;
    private Rigidbody rb;
    private CapsuleCollider collider;
    private PlayerCamera cam;

    private Transform orientation;
    private Vector3 moveDirection;

    private Vector2 mouseInput;
    
    public Player()
    {
        GameManager.GlobalAwake += Awake;
        GameManager.GlobalStart += Start;
        GameManager.GlobalUpdate += Update;
        GameManager.GlobalFixedUpdate += FixedUpdate;
    }

    private void Awake()
    {
        playerObject = new GameObject();
        
        rb = playerObject.AddComponent<Rigidbody>();
        collider = playerObject.AddComponent<CapsuleCollider>();
        
        collider = new CapsuleCollider
        {
            center = new Vector3(0, 1, 0)
        };

        cam = new PlayerCamera(playerObject);

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        
        input = new InputActions();

        input.Player.MouseXY.performed += SetMouse;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        cam.UpdateCamera(mouseInput, collider.transform);
    }

    private void SetMouse(InputAction.CallbackContext value)
    {
        mouseInput = value.ReadValue<Vector2>();
    }

    private void MovePlayer(InputAction.CallbackContext value)
    {
        
    }
}
