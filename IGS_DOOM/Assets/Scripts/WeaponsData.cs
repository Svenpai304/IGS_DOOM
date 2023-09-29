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
    public int MaxAmmo;
    public int Ammo;

    public FireBehaviour CurrentMod;

    public bool Unlocked;

    public bool Mod1Unlocked;
    public bool Mod2Unlocked;

    public int Mod1Upgrade1Level;
    public int Mod1Upgrade2Level;
    public int Mod1Upgrade3Level;
    public bool Mod1MasteryUnlocked;

    public int Mod2Upgrade1Level;
    public int Mod2Upgrade2Level;
    public int Mod2Upgrade3Level;
    public bool Mod2MasteryUnlocked;
}
