using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : IDamageable, IGloryKillable
{
    private int health = 20;
    private GameObject enemyObj;
    private int gloryKillThreshold = 5;

    public EnemyController(GameObject _enemyObj)
    {
        enemyObj = _enemyObj;
    }
    public void TakeDamage(int damage)
    {
        Debug.Log($"{enemyObj.name} takes {damage} damage");
        health -= damage;
        Debug.Log(IsInStaggerState(health));
        if (IsInStaggerState(health))
        {
            GameManager.Instance.StartCoroutine(GloryKill());
        }
        if(health <= 0)
        {
            EnemyManager.EnemyDict.Remove(enemyObj.name);
            Object.Destroy(enemyObj);
        }
    }

    public bool IsInStaggerState(int health)
    {
        if (health > gloryKillThreshold)
        {
            return false;
        }
        return true;
    }

    public IEnumerator GloryKill()
    {
        // Logic for Animation events etc for Glory kills
        Debug.Log("GloryKillStarted");
        yield return new WaitForSeconds(.5f);
        EnemyManager.EnemyDict.Remove(enemyObj.name);
        Object.Destroy(enemyObj);
        Debug.Log("GloryKillEnded");
    }
}

