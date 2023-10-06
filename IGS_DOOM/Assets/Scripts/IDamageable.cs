using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);

    public bool IsInStaggerState(int health);

}

public interface IGloryKillable
{
    public IEnumerator GloryKill();
}

