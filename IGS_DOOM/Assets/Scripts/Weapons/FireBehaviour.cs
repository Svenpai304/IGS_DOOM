using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBehaviour", menuName = "Fire Behaviour", order = 1)]
public class FireBehaviour : ScriptableObject
{
    [SerializeField] private FireControlComponent fireControlComponent;
    [SerializeField] public List<FireComponent> FireComponents = new List<FireComponent>();
    [HideInInspector] public Vector3[] lastFireDirections;

    public void OnSwitchIn(Weapon _weapon)
    {
        fireControlComponent.OnSwitchIn(_weapon, this);
    }

    public void OnSwitchOut(Weapon _weapon)
    {
        fireControlComponent.OnSwitchOut(_weapon, this);
    }

    public void ActivateFireComponents(Weapon _weapon, Vector3 fireDirection)
    {
        if(_weapon.Data.Ammo <= 0) { return; }
        foreach (var component in FireComponents)
        {
            var dirs = component.Fire(_weapon, this, fireDirection);
            if(dirs != null)
            {
                lastFireDirections = dirs;
            }
        }
    }

    public List<UpgradeableValue> GetUpgradeableValues()
    {
        List<UpgradeableValue> values = new List<UpgradeableValue>();

        if (fireControlComponent != null)
        {
            foreach (var value in fireControlComponent.allUpgradeableValues)
            {
                values.Add(value);
            }
        }
        foreach (var component in FireComponents)
        {
            if (component != null)
            {
                foreach (var value in component.allUpgradeableValues)
                {
                    values.Add(value);
                }
            }
        }
        return values;
    }
}
