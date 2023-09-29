using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleProjectile : FireComponent
{
    [SerializeField] private GameObject projectilePrefab;
    public override void Fire(Weapon _weapon)
    {
        _ = new ProjectileController(Instantiate(projectilePrefab));
    }
}
