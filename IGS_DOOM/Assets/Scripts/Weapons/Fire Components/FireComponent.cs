using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireComponent : ScriptableObject
{
    [HideInInspector] public List<UpgradeableValue> allUpgradeableValues = new List<UpgradeableValue>();
    [SerializeField] private int ammoCost;
    public abstract void Fire(Weapon _weapon);
}
