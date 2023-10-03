using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBehaviour", menuName = "Fire Behaviour", order = 1)]
public class FireBehaviour : ScriptableObject
{
    [SerializeField]private FireControlComponent fireControlComponent;
    [SerializeField]public List<FireComponent> FireComponents = new List<FireComponent>();

    public void OnSwitchIn(Weapon _weapon)
    {
        fireControlComponent.OnSwitchIn(_weapon, this);
    }

    public void OnSwitchOut(Weapon _weapon)
    {
        fireControlComponent.OnSwitchOut(_weapon, this);
    }

    public void ActivateFireComponents(Weapon _weapon)
    {
        foreach (var component in FireComponents)
        {
            component.Fire(_weapon);
        }
    }

    public List<UpgradeableValue> GetUpgradeableValues()
    {
        List<UpgradeableValue> values = new List<UpgradeableValue>();

        foreach(var value in fireControlComponent.allUpgradeableValues)
        {
            values.Add(value);
        }
        foreach (var component in FireComponents)
        {
            foreach(var value in component.allUpgradeableValues)
            {
                values.Add(value);
            }
        }
        return values;
    }
}
