using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponsData
{
    public static List<IWeapon> Weapons = new List<IWeapon>();
}

public class WeaponData
{
    public int Ammo;

    public bool Unlocked;

    public bool Mod1Unlocked;
    public bool Mod2Unlocked;
}
