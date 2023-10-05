using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleProjectile : FireComponent
{
    [SerializeField] private GameObject projectilePrefab;
    public override Vector3[] Fire(Weapon _weapon, Vector3 fireDirection)
    {
        _ = new ProjectileController(Instantiate(projectilePrefab));

        Vector3[] dirs = new Vector3[1];
        dirs[0] = fireDirection;
        return dirs;
    }
}
