using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpreadHitscan : FireComponent
{
    [SerializeField] private UpgradeableValue damage;
    [SerializeField] private int count;
    [SerializeField] private float spread;

    public SpreadHitscan()
    {
        allUpgradeableValues.Add(damage);
    }

    public override Vector3[] Fire(Weapon _weapon, FireBehaviour fireBehaviour, Vector3 fireDirection)
    {
        Vector3[] dirs = new Vector3[count];
        for(int i = 0; i < count; i++)
        {
            Vector3 randFireDir = AddSpreadToVector(fireDirection, spread);
            WeaponUtil.FireHitscan(_weapon, randFireDir, damage);
            dirs[i] = randFireDir;
        }
        return dirs;
    }

    public static Vector3 AddSpreadToVector(Vector3 startDir, float maxSpread)
    {
        return startDir + Random.insideUnitSphere * maxSpread;
    }
}
