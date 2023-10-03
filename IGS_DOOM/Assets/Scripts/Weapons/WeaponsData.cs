using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class WeaponsData : ScriptableObject
{
    public List<Weapon> Weapons = new List<Weapon>();
}

[Serializable]
public class WeaponData
{
    public int MaxAmmo;
    public int Ammo;

    [HideInInspector] public int CurrentMod;
    [HideInInspector] public IWeaponHolder Owner;

    public bool Unlocked;

    public bool[] ModsUnlocked;
    [HideInInspector] public UpgradeableValue[][] Upgradeables;
    [HideInInspector] public int[][] UpgradeLevels;

    public int[] Mod1UpgradeLevels = new int[3];
    public int[] Mod2UpgradeLevels = new int[3];
    public bool Mod1MasteryUnlocked;
    public bool Mod2MasteryUnlocked;
}
