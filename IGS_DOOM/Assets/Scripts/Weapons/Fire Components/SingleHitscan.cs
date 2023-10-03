using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleHitscan", menuName = "Fire Component: Single Hitscan", order = 3)]
public class SingleHitscan : FireComponent
{
    [SerializeField] private int damage;
    [SerializeField] private float maxDistance;

    public override void Fire(Weapon _weapon)
    {
        Transform cam = _weapon.Data.Owner.CamTransform;
        RaycastHit hit;
        Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, LayerMask.GetMask("Damageable"));
        if(hit.collider == null ) { return; }
        if (EnemyManager.EnemyDict.ContainsKey(hit.collider.name))
        {
            EnemyManager.EnemyDict[hit.collider.name].TakeDamage(damage);
        }
    }
}
