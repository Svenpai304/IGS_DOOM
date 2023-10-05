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

    public override Vector3[] Fire(Weapon _weapon, Vector3 fireDirection)
    {
        WeaponUtil.FireHitscan(_weapon, fireDirection, damage);
        Vector3[] dirArray = new Vector3[1];
        dirArray[0] = fireDirection;
        return dirArray;
    }
}
