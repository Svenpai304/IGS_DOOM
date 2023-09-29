using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WeaponCarrier
{
    public List<IWeapon> Weapons = new List<IWeapon>();
    public IWeapon CurrentWeapon;
    public int CurrentIndex;
    public int PreviousIndex;

    public WeaponCarrier() 
    {
        var weaponsData = Resources.Load<WeaponsData>("WeaponsData");
        for(int i = 0; i < weaponsData.Weapons.Count; i++)
        {
            Weapons.Add(weaponsData.Weapons[i]);
            Debug.Log("Added weapon " + i);
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

    }

    public void SwitchWeaponMod()
    {
        CurrentWeapon.SwitchMod();
    }

    public void SwitchToNextWeapon()
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
