using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeableValue
{
    public bool IsUpgradeable;
    public int UpgradeIndex;
    private int level;
    [SerializeField] private float[] values;

    public float GetValue()
    {
        return values[level];
    }

    public void SetUpgradeLevel(int _level)
    {
        level = _level;
    }
}
