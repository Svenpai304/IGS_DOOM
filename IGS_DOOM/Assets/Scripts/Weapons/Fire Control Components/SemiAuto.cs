using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : FireControlComponent
{
    [SerializeField] private float spread;
    private FireBehaviour behaviour;
    public override void OnSwitchIn(Weapon weapon, FireBehaviour fireBehaviour)
    {
        weapon.OnFirePressed += Fire;
    }

    public override void OnSwitchOut(Weapon weapon, FireBehaviour fireBehaviour)
    {
        weapon.OnFirePressed -= Fire;
    }

    private void Fire(Weapon weapon)
    {
        behaviour.ActivateFireComponents(weapon, WeaponUtil.AddSpreadToVector3(weapon.Data.Owner.CamTransform.forward, spread));
    }
}
