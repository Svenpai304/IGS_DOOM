using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : ScriptableObject
{
    [SerializeField]public List<FireComponent> FireComponents = new List<FireComponent>();

    public void OnSwitchIn(Weapon weapon)
    {
        foreach (var component in FireComponents)
        {
            component.OnSwitchIn(weapon);
        }
    }

    public void OnSwitchOut(Weapon weapon)
    {
        foreach (var component in FireComponents)
        {
            component.OnSwitchOut(weapon);
        }
    }
}
