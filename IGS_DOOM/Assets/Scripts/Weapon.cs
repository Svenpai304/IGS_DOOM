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

    public void Fire()
    {
        throw new System.NotImplementedException();
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
