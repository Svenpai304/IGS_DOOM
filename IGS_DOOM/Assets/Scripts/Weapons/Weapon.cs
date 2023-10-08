using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseWeapon", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject, IWeapon
{
    [SerializeField] private WeaponData data;
    public WeaponData Data { get => data; set => data = value; }

    [SerializeField] private GameObject viewmodelPrefab;
    public GameObject ViewmodelPrefab { get => viewmodelPrefab; set => viewmodelPrefab = value; }
    private GameObject activeViewmodel;

    [SerializeField] public FireBehaviour primaryFire;
    [SerializeField] public FireBehaviour[] altFires;

    public Action<Weapon> OnFirePressed;
    public Action<Weapon> OnFireReleased;
    public Action<Weapon> OnAltFirePressed;
    public Action<Weapon> OnAltFireReleased;

    public void FirePressed()
    {
        OnFirePressed?.Invoke(this);
    }

    public void FireReleased()
    {
        OnFireReleased?.Invoke(this);
    }

    public void AltFirePressed()
    {
        OnAltFirePressed?.Invoke(this);
    }

    public void AltFireReleased()
    {
        OnAltFireReleased?.Invoke(this);
    }

    public void OnSwitchIn()
    {
        activeViewmodel = Instantiate(ViewmodelPrefab, data.Owner.WeaponTransform);
        if (primaryFire != null)
        {
            primaryFire.OnSwitchIn(this);
        }
        if (Data.CurrentMod != 0)
        {
            altFires[Data.CurrentMod - 1].OnSwitchIn(this);
        }
    }

    public void OnSwitchOut()
    {
        Destroy(activeViewmodel); 
        if (primaryFire != null)
        {
            primaryFire.OnSwitchOut(this);
        }
        if (Data.CurrentMod != 0)
        {
            altFires[Data.CurrentMod - 1].OnSwitchOut(this);
        }
    }

    public void SwitchMod()
    {
        int newMod = Data.CurrentMod + 1;
        if (newMod > 2)
        {
            newMod = 1;
        }
        if (!Data.ModsUnlocked[newMod - 1]) { return; }

        if (Data.CurrentMod != 0)
        {
            altFires[Data.CurrentMod - 1].OnSwitchOut(this);
        }
        Data.CurrentMod = newMod;
        altFires[Data.CurrentMod - 1].OnSwitchIn(this);
    }

    public void DisablePrimaryFire()
    {
        Debug.Log("Primary fire disabled");
        primaryFire.OnSwitchOut(this);
    }

    public void EnablePrimaryFire()
    {
        Debug.Log("Primary fire re-enabled");
        primaryFire.OnSwitchIn(this);
    }

    public void CollectUpgradeableValues()
    {
        for(int i = 0; i < altFires.Length; i++)
        {
            foreach(var value in altFires[i].GetUpgradeableValues())
            {
                if (value.IsUpgradeable)
                {
                    Data.Upgradeables[i][value.UpgradeIndex] = value;
                    value.SetUpgradeLevel(Data.UpgradeLevels[i][value.UpgradeIndex]);
                }
            }
        }
    }
}
