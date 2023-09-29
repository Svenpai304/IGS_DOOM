using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : FireControlComponent
{
    public override void OnSwitchIn(Weapon weapon, FireBehaviour fireBehaviour)
    {
        weapon.OnFirePressed += fireBehaviour.ActivateFireComponents;
    }

    public override void OnSwitchOut(Weapon weapon, FireBehaviour fireBehaviour)
    {
        weapon.OnFirePressed -= fireBehaviour.ActivateFireComponents;
    }
}
