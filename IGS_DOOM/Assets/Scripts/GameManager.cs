using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public delegate void Action();
    public static Action GlobalUpdate;

    private WeaponCarrier weapons;

    private void Start()
    {       
        var a = new InputManager();
        weapons = new WeaponCarrier();
    }

    private void Update()
    {
        GlobalUpdate?.Invoke();
    }
}
