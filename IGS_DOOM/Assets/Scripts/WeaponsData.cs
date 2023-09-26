using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsData : ScriptableObject
{
    public List<Weapon> Weapons = new List<Weapon>();
}

[Serializable]
public class WeaponData
{
    public int Ammo;

    public bool Unlocked;

    public bool Mod1Unlocked;
    public bool Mod2Unlocked;
}
