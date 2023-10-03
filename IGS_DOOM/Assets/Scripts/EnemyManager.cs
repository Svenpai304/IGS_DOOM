using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    // Instantiate enemies with keys as the name
    // Place them in the world and attach scripts
    // choose one or the other
    
    // put into dictionary with specefied key

    // Enemies are hit with the hit.collider.name
    // so when hit request the enemy corresponding with the key

    public static Dictionary<string, IDamageable> EnemyDict = new Dictionary<string, IDamageable>();

    public EnemyManager(LayerMask layer)
    {
        FindCollidersByLayer(layer);
    }

    private void FindCollidersByLayer(LayerMask layer)
    {
        var objectArray = Object.FindObjectsOfType<Collider>();
        Debug.Log(objectArray.Length + " colliders found");
        foreach (var obj in objectArray)
        {
            if (layer == (layer | (1 << obj.gameObject.layer)))
            {
                if(EnemyDict.ContainsKey(obj.name))
                {
                    obj.name += 1;
                }
                EnemyDict.Add(obj.name, new EnemyController(obj.gameObject));
            }
        }
        Debug.Log($"Enemy list count: {EnemyDict.Count}");
    }
}
