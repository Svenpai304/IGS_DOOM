using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleHitscan", menuName = "Fire Component: Single Hitscan", order = 3)]
public class SingleHitscan : FireComponent
{
    [SerializeField] private UpgradeableValue damage;
    [SerializeField] private float maxDistance;

    public SingleHitscan()
    {
        allUpgradeableValues.Add(damage);
    }

    public override Vector3[] Fire(Weapon _weapon, FireBehaviour fireBehaviour, Vector3 fireDirection)
    {
        _weapon.Data.Ammo -= ammoCost;
        _weapon.Data.Ammo = Mathf.Clamp(ammoCost, 0, _weapon.Data.MaxAmmo);
        WeaponUtil.FireHitscan(_weapon, fireDirection, damage);
        Vector3[] dirArray = new Vector3[1];
        dirArray[0] = fireDirection;
        return dirArray;
    }
}
