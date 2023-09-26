using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponCarrier
{
    public List<IWeapon> Weapons;
    public IWeapon CurrentWeapon;
    public int CurrentIndex;
    public int PreviousIndex;

    public WeaponCarrier() 
    {
        Weapons = WeaponsData.Weapons;
    }

    public void Fire()
    {
        CurrentWeapon.Fire();
    }

    public void SwitchWeapon(int i)
    {
        CurrentWeapon = Weapons[i];
        PreviousIndex = CurrentIndex;
        CurrentIndex = i;
    }

    public void SwitchWeaponMod()
    {
        CurrentWeapon.SwitchMod();
    }

    public void SwitchToNextWeapon()
    {
        for(int i = CurrentIndex + 1; i == CurrentIndex; i++)
        {
            if(i > Weapons.Count)
            {
                break;
            }
            if (WeaponsData.DataDict[WeaponsData.Weapons[i]].Unlocked)
            {
                SwitchWeapon(i);
            }
        }
    }

    public void SwitchToPreviousWeapon() 
    {
        SwitchWeapon(PreviousIndex);
    }

    public void UnlockWeapon(int i)
    {
        if (!WeaponsData.DataDict[WeaponsData.Weapons[i]].Unlocked) 
        {
            WeaponsData.DataDict[WeaponsData.Weapons[i]].Unlocked = true;
            SwitchWeapon(i);
        }
    }


}
