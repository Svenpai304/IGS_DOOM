using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleHitscan", menuName = "Fire Component: Single Hitscan", order = 3)]
public class SingleHitscan : FireComponent
{
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;

    public override void Fire(Weapon _weapon)
    {
        Debug.Log($"Hitscan weapon fired: {damage} damage dealt");
    }
}
