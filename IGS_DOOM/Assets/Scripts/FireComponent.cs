using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireComponent : ScriptableObject
{
    public abstract void OnSwitchIn(Weapon weapon);
    public abstract void OnSwitchOut(Weapon weapon);
}
