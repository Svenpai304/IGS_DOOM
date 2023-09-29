using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponInputHandler
{
    public abstract void FirePressed(Weapon weapon);
    public abstract void FireReleased(Weapon weapon);
    public abstract void AltFirePressed(Weapon weapon);
    public abstract void AltFireReleased(Weapon weapon);
}
