using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public static WeaponData data;
    public void Fire();
    public void SwitchMod();
}
