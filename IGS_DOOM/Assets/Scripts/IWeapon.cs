using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public WeaponData Data { get; set; }
    public GameObject ViewmodelPrefab { get; set; }
    public void Fire();
    public void SwitchMod();
    public void OnSwitchIn();
    public void OnSwitchOut();
}
