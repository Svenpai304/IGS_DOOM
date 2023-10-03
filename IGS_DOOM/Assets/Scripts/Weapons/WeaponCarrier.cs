using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Weapons;

public class WeaponCarrier
{
    public List<IWeapon> Weapons = new List<IWeapon>();
    public IWeapon CurrentWeapon;
    public int CurrentIndex = 0;
    public int PreviousIndex;

    private IWeaponHolder player;

    public WeaponCarrier(IWeaponHolder _player) 
    {
        player = _player;
        var weaponsData = Resources.Load<WeaponsData>("WeaponsData");
        for(int i = 0; i < weaponsData.Weapons.Count; i++)
        {
            Weapons.Add(weaponsData.Weapons[i]);
            Weapons[i].Data.Owner = player;
        }
        SwitchWeapon(0);
    }

    public void SwitchWeapon(int i)
    {
        CurrentWeapon = Weapons[i];
        PreviousIndex = CurrentIndex;
        CurrentIndex = i;
        Weapons[PreviousIndex].OnSwitchOut();
        CurrentWeapon.OnSwitchIn();
        Debug.Log(CurrentIndex);
    }

    public void SwitchWeaponMod()
    {
        CurrentWeapon.SwitchMod();
    }

    public void SwitchWeaponForward()
    {
        for(int i = CurrentIndex + 1; i != CurrentIndex; i++)
        {
            if(i >= Weapons.Count)
            {
                i = 0;
            }
            if (Weapons[i].Data.Unlocked)
            {
                SwitchWeapon(i);
                return;
            }
        }
    }

    public void SwitchWeaponBackward()
    {
        for (int i = CurrentIndex - 1; i != CurrentIndex; i--)
        {
            if (i < 0)
            {
                i = Weapons.Count;
            }
            if (Weapons[i].Data.Unlocked)
            {
                SwitchWeapon(i);
                return;
            }
        }
    }

    public void SwitchToPreviousWeapon() 
    {
        SwitchWeapon(PreviousIndex);
    }

    public void UnlockWeapon(int i)
    {
        if (Weapons[i].Data.Unlocked) 
        {
            Weapons[i].Data.Unlocked = true;
            SwitchWeapon(i);
        }
    }


}
