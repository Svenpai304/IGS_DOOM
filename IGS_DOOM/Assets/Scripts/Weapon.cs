using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public FireBehaviour altFire1;
    [SerializeField] public FireBehaviour altFire2;

    private Action<Weapon> OnFirePressed;
    private Action<Weapon> OnFireReleased;
    private Action<Weapon> OnAltFirePressed;
    private Action<Weapon> OnAltFireReleased;
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
        activeViewmodel = Instantiate(ViewmodelPrefab);
    }

    public void OnSwitchOut()
    {
        Destroy(activeViewmodel);
    }

    public void SwitchMod()
    {
        throw new System.NotImplementedException();
    }

}
