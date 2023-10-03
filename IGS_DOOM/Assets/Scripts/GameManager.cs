using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public delegate void Action();
    public static Action GlobalAwake;
    public static Action GlobalStart;
    public static Action GlobalUpdate;
    public static Action GlobalFixedUpdate;
    public static Action GlobalOnEnable;
    public static Action GlobalOnDisable;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }
        
        var a = new Player.Player();
        weapons = new WeaponCarrier(a);
        GlobalAwake?.Invoke();
    }

    private WeaponCarrier weapons;

    [ContextMenu("Fire weapon debug")]
    public void FireWeaponDebug()
    {
        StartCoroutine(FireWeaponDebugCoroutine());
    }
    private IEnumerator FireWeaponDebugCoroutine()
    {
        Debug.Log("Start firing");
        weapons.CurrentWeapon.FirePressed();
        yield return new WaitForSeconds(1);
        weapons.CurrentWeapon.FireReleased();
    }

    private void OnEnable()
    {
        GlobalOnEnable?.Invoke();
    }
    
    private void OnDisable()
    {
        GlobalOnDisable?.Invoke();
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
