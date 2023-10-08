using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FullAuto", menuName = "Fire Control: Full Auto", order = 2)]
public class FullAuto : FireControlComponent
{
    [SerializeField] private float interval;
    [SerializeField] private float spread;

    private bool isAutoActive;
    private float intervalTimer;

    private Weapon weapon;
    private FireBehaviour fireBehaviour;

    public override void OnSwitchIn(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnFirePressed += StartAuto;
        _weapon.OnFireReleased += StopAuto;
        GameManager.GlobalFixedUpdate += FixedUpdate;
        Debug.Log("FA activated");

        weapon = _weapon;
        fireBehaviour = _fireBehaviour;
        isAutoActive = false;
    }

    public override void OnSwitchOut(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnFirePressed -= StartAuto;
        _weapon.OnFireReleased -= StopAuto;
        GameManager.GlobalFixedUpdate -= FixedUpdate;
        Debug.Log("FA deactivated");

        intervalTimer = 0;
        isAutoActive = false;
    }

    private void FixedUpdate()
    {
        intervalTimer -= Time.fixedDeltaTime;
        intervalTimer = Mathf.Clamp(intervalTimer, 0, interval);

        if (intervalTimer == 0 && isAutoActive)
        {
            fireBehaviour.ActivateFireComponents(weapon, WeaponUtil.AddSpreadToVector3(weapon.Data.Owner.CamTransform.forward, spread));
            intervalTimer = interval;
        }
    }

    private void StartAuto(Weapon _weapon)
    {
        isAutoActive = true;
    }

    private void StopAuto(Weapon _weapon)
    {
        isAutoActive = false;
    }
}
