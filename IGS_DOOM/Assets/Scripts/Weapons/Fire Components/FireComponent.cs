using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireComponent : ScriptableObject
{
    [HideInInspector] public List<UpgradeableValue> allUpgradeableValues = new List<UpgradeableValue>();
    [SerializeField] protected int ammoCost;
    public abstract Vector3[] Fire(Weapon _weapon, FireBehaviour fireBehaviour, Vector3 fireDirection);
}
