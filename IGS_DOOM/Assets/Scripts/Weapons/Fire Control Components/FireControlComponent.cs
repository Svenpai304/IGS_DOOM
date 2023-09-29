using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireControlComponent : ScriptableObject
{
    public abstract void OnSwitchIn(Weapon _weapon, FireBehaviour _fireBehaviour);
    public abstract void OnSwitchOut(Weapon _weapon, FireBehaviour _fireBehaviour);
}
