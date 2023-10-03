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

    public override void Fire(Weapon _weapon)
    {
        Transform cam = _weapon.Data.Owner.CamTransform;
        RaycastHit hit;
        if (!Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, LayerMask.GetMask("Damageable"))) { return; }

        if (EnemyManager.EnemyDict.ContainsKey(hit.collider.name))
        {
            EnemyManager.EnemyDict[hit.collider.name].TakeDamage((int)damage.GetValue());
        }
    }
}
