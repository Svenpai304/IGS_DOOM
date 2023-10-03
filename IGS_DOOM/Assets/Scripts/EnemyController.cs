using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : IDamageable
{
    private int health = 20;
    private GameObject enemyObj;

    public EnemyController(GameObject _enemyObj)
    {
        enemyObj = _enemyObj;
    }
    public void TakeDamage(int damage)
    {
        Debug.Log($"{enemyObj.name} takes {damage} damage");
        health -= damage;
        if(health <= 0)
        {
            EnemyManager.EnemyDict.Remove(enemyObj.name);
            Object.Destroy(enemyObj);
        }
    }
}
