using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public WeaponData Data { get; set; }
    public GameObject ViewmodelPrefab { get; set; }

    public void FirePressed();
    public void FireReleased();
    public void AltFirePressed();
    public void AltFireReleased();
    public void SwitchMod();
    public void OnSwitchIn();
    public void OnSwitchOut();
    public void CollectUpgradeableValues();
}
