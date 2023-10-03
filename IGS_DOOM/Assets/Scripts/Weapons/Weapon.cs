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

    public delegate void WeaponAction(Weapon _weapon);
    public WeaponAction OnFirePressed;
    public WeaponAction OnFireReleased;
    public WeaponAction OnAltFirePressed;
    public WeaponAction OnAltFireReleased;
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
        if (primaryFire == null) { return; }
        activeViewmodel = Instantiate(ViewmodelPrefab, data.Owner.WeaponTransform);
        primaryFire.OnSwitchIn(this);
        if (Data.CurrentMod != 0)
        {
            altFires[Data.CurrentMod - 1].OnSwitchIn(this);
        }
    }

    public void OnSwitchOut()
    {
        if (primaryFire == null) { return; }
        Destroy(activeViewmodel);
        primaryFire.OnSwitchOut(this);
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

}
