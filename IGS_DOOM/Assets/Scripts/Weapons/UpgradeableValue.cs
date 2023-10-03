using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeableValue
{
    public bool IsUpgradeable;
    public int UpgradeIndex;
    [SerializeField] private float[] values;
    public int Level;

    public float GetValue()
    {
        return values[Level];
    }

    public void SetUpgradeLevel(int _level)
    {
        Level = _level;
    }
}
