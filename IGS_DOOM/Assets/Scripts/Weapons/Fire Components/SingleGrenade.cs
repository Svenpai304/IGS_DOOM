using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class SingleGrenade : FireComponent
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GrenadeData grenadeData;
    [SerializeField] private float startForce;
    [SerializeField] private float startOffset;
    public override Vector3[] Fire(Weapon _weapon, FireBehaviour fireBehaviour, Vector3 fireDirection)
    {
        Debug.Log("Grenade launched");
        _weapon.Data.Ammo -= ammoCost;
        _weapon.Data.Ammo = Mathf.Clamp(ammoCost, 0, _weapon.Data.MaxAmmo);
        Vector3 startPos = _weapon.Data.Owner.CamTransform.position + (startOffset * _weapon.Data.Owner.CamTransform.forward);
        Vector3 fireForce = fireDirection * startForce;

        _ = new GrenadeController(Instantiate(grenadePrefab, startPos, _weapon.Data.Owner.CamTransform.rotation), fireForce, grenadeData);

        Vector3[] dirs = new Vector3[1];
        dirs[0] = fireDirection;
        return dirs;
    }

    [Serializable]
    public struct GrenadeData
    {
        public GameObject explosionPrefab;
        public float explodeEffectDuration;
        public float hitRadius;
        public UpgradeableValue explodeRadius;
        public int damage;
    }
}
