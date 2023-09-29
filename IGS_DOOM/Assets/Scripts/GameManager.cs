using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public delegate void Action();
    public static Action GlobalAwake;
    public static Action GlobalStart;
    public static Action GlobalUpdate;
    public static Action GlobalFixedUpdate;

    private void Awake()
    {
        var a = new Player();
        var b = new WeaponCarrier();
        GlobalAwake?.Invoke();
    }

    private void Start()
    {
        GlobalStart?.Invoke();
    }

    private void Update()
    {
        GlobalUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        GlobalFixedUpdate?.Invoke();
    }
}
