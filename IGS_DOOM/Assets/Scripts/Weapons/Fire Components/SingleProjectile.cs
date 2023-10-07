using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleProjectile : FireComponent
{
    [SerializeField] private GameObject projectilePrefab;
    public override Vector3[] Fire(Weapon _weapon, FireBehaviour fireBehaviour, Vector3 fireDirection)
    {
        _weapon.Data.Ammo -= ammoCost;
        _weapon.Data.Ammo = Mathf.Clamp(ammoCost, 0, _weapon.Data.MaxAmmo);
        _ = new ProjectileController(Instantiate(projectilePrefab));

        Vector3[] dirs = new Vector3[1];
        dirs[0] = fireDirection;
        return dirs;
    }
}
