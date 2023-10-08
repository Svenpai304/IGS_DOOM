using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AltSemiAuto : FireControlComponent
{
    [SerializeField] private UpgradeableValue loadTime;
    [SerializeField] private UpgradeableValue rechargeTime;
    [SerializeField] private float spread;

    private FireBehaviour fireBehaviour;

    private float timer;

    private bool isLoaded;
    private bool isRecharging;
    private bool fireModeActive;

    public override void OnSwitchIn(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnAltFirePressed += EnterFireMode;
        _weapon.OnAltFireReleased += ExitFireMode;
        GameManager.GlobalFixedUpdate += FixedUpdate;
        fireBehaviour = _fireBehaviour;
    }

    public override void OnSwitchOut(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnAltFirePressed -= EnterFireMode;
        _weapon.OnAltFireReleased -= ExitFireMode;
        GameManager.GlobalFixedUpdate -= FixedUpdate;
        if (fireModeActive)
        {
            ExitFireMode(_weapon);
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(isLoaded);
        if(isLoaded) { return; }
        timer += Time.fixedDeltaTime;
        if(isRecharging && timer > rechargeTime.GetValue())
        {
            isRecharging = false;
            timer = 0f;
        }
        else if(!isRecharging && timer > loadTime.GetValue())
        {
            isLoaded = true;
            timer = 0f;
        }
    }

    private void EnterFireMode(Weapon _weapon)
    {
        _weapon.DisablePrimaryFire();
        _weapon.OnFirePressed += Fire;
        fireModeActive = true;
    }

    private void ExitFireMode(Weapon _weapon)
    {
        _weapon.EnablePrimaryFire();
        _weapon.OnFirePressed -= Fire;
        fireModeActive = false;
    }

    private void Fire(Weapon _weapon)
    {
        if (isLoaded)
        {
            Debug.Log("Firing grenade");
            fireBehaviour.ActivateFireComponents(_weapon, WeaponUtil.AddSpreadToVector3(_weapon.Data.Owner.CamTransform.forward, spread));
            isLoaded = false;
            isRecharging = true;
            timer = 0;
        }
    }
}
