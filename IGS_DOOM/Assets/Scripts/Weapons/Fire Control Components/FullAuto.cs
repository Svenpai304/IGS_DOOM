using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FullAuto", menuName = "Fire Control: Full Auto", order = 2)]
public class FullAuto : FireControlComponent
{
    [SerializeField] private float interval;

    private bool isAutoActive;
    private float intervalTimer;

    private Weapon weapon;
    private FireBehaviour fireBehaviour;

    public override void OnSwitchIn(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnFirePressed += StartAuto;
        _weapon.OnFireReleased += StopAuto;
        GameManager.GlobalUpdate += FixedUpdate;
        weapon = _weapon;
        fireBehaviour = _fireBehaviour;
    }

    public override void OnSwitchOut(Weapon _weapon, FireBehaviour _fireBehaviour)
    {
        _weapon.OnFirePressed -= StartAuto;
        _weapon.OnFireReleased -= StopAuto;
        GameManager.GlobalFixedUpdate -= FixedUpdate;
        intervalTimer = 0;
    }

    private void FixedUpdate()
    {
        intervalTimer -= Time.deltaTime;
        intervalTimer = Mathf.Clamp(intervalTimer, 0, interval);

        if (intervalTimer == 0 && isAutoActive)
        {
            fireBehaviour.ActivateFireComponents(weapon);
            intervalTimer = interval;
        }
    }

    private void StartAuto(Weapon _weapon)
    {
        if(intervalTimer == 0)
        {
            intervalTimer = interval;
        }
        isAutoActive = true;
    }

    private void StopAuto(Weapon _weapon)
    {
        isAutoActive = false;
    }
}
