using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AltBurstFire : FireControlComponent
{
    [SerializeField] private int maxBurstCount;
    [SerializeField] private UpgradeableValue burstChargeTime;
    [SerializeField] private UpgradeableValue burstResetTime;
    [SerializeField] private UpgradeableValue burstFireRate;

    private int currentBurstCount;
    private float timer;

    private enum BurstStateEnum { Charging, Resetting, Firing, Idle }
    private BurstStateEnum state;
    private bool fireModeActive;

    private FireBehaviour fireBehaviour;
    private Weapon weapon;

    public AltBurstFire()
    {
        allUpgradeableValues.Add(burstChargeTime);
        allUpgradeableValues.Add(burstResetTime);
        allUpgradeableValues.Add(burstFireRate);
    }

    public override void OnSwitchIn(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnAltFirePressed += EnterFireMode;
        _weapon.OnAltFireReleased += ExitFireMode;
        weapon = _weapon;
        fireBehaviour = _fireBehaviour;
        state = BurstStateEnum.Idle;
    }

    public override void OnSwitchOut(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnAltFirePressed -= EnterFireMode;
        _weapon.OnAltFireReleased -= ExitFireMode;
        GameManager.GlobalFixedUpdate -= FixedUpdate;
        ExitFireMode(_weapon);
    }

    public void FixedUpdate()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case BurstStateEnum.Firing: FiringBehaviour(); break;
            case BurstStateEnum.Charging: ChargingBehaviour(); break;
            case BurstStateEnum.Resetting: ResettingBehaviour(); break;
        }

    }

    private void FiringBehaviour()
    {
        if(currentBurstCount == 0)
        {
            state = BurstStateEnum.Charging;
        }

        if (timer >= burstFireRate.GetValue())
        {
            currentBurstCount--;
            fireBehaviour.ActivateFireComponents(weapon, WeaponUtil.AddSpreadToVector3(weapon.Data.Owner.CamTransform.forward, 0));
            if (currentBurstCount <= 0)
            {
                state = BurstStateEnum.Resetting;
            }
            timer = 0;
        }
    }

    private void ChargingBehaviour()
    {
        if (timer >= burstChargeTime.GetValue())
        {
            currentBurstCount += 1;
            currentBurstCount = Mathf.Clamp(currentBurstCount, 0, maxBurstCount);
            timer = 0;
        }
    }

    private void ResettingBehaviour()
    {
        if (timer >= burstResetTime.GetValue())
        {
            state = BurstStateEnum.Charging;
            timer = 0;
            if (!fireModeActive)
            {
                state = BurstStateEnum.Idle;
            }
        }
    }

    private void EnterFireMode(Weapon _weapon)
    {
        GameManager.GlobalFixedUpdate += FixedUpdate;
        _weapon.DisablePrimaryFire();
        _weapon.OnFirePressed += FireBurst;
        timer = 0;
        state = BurstStateEnum.Charging;
        fireModeActive = true;
    }

    private void ExitFireMode(Weapon _weapon)
    {
        _weapon.EnablePrimaryFire();
        _weapon.OnFirePressed -= FireBurst;
        currentBurstCount = 0;
        fireModeActive = false;
    }

    private void FireBurst(Weapon _weapon)
    {
        if (state == BurstStateEnum.Charging && currentBurstCount == maxBurstCount)
        {
            state = BurstStateEnum.Firing;
        }
    }
}
