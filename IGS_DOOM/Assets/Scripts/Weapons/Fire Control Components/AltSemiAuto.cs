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
        fireBehaviour = _fireBehaviour;
    }

    public override void OnSwitchOut(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnAltFirePressed -= EnterFireMode;
        _weapon.OnAltFireReleased -= ExitFireMode;
        ExitFireMode(_weapon);
    }

    private void FixedUpdate()
    {
        Debug.Log(isLoaded);
        timer += Time.deltaTime;
        if(isRecharging && timer > rechargeTime.GetValue())
        {
            isRecharging = false;
            timer = 0f;
        }
        else if(!isLoaded && timer > loadTime.GetValue())
        {
            isLoaded = true;
            timer = 0f;
        }
        if(!isRecharging && !fireModeActive)
        {
            GameManager.GlobalFixedUpdate -= FixedUpdate;
        }
    }

    private void EnterFireMode(Weapon _weapon)
    {
        Debug.Log("Entered alt fire");
        GameManager.GlobalFixedUpdate += FixedUpdate;
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
        }
    }
}
