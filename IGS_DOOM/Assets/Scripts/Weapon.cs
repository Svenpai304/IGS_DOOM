using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject, IWeapon
{
    public static WeaponData data;

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void SwitchMod()
    {
        throw new System.NotImplementedException();
    }
}
