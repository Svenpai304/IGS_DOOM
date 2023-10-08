using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponUtil
{
    public static void FireHitscan(Weapon weapon, Vector3 fireDirection, UpgradeableValue damage)
    {
        Transform cam = weapon.Data.Owner.CamTransform;
        RaycastHit hit;
        if (!Physics.Raycast(cam.position, fireDirection, out hit, 1000, LayerMask.GetMask("Damageable"))) { return; }

        if (EnemyManager.EnemyDict.ContainsKey(hit.collider.name))
        {
            EnemyManager.EnemyDict[hit.collider.name].TakeDamage((int)damage.GetValue());
        }
    }

    public static Vector3 AddSpreadToVector3(Vector3 startDir, float maxSpread)
    {
        return startDir + UnityEngine.Random.insideUnitSphere * maxSpread;
    }
}
