using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class SpawnDecals : FireComponent
{
    [SerializeField] private GameObject decalPrefab;
    [SerializeField] private int maxAmount;
    private ObjectPool<DecalController> decalPool = null;

    public override Vector3[] Fire(Weapon _weapon, FireBehaviour fireBehaviour, Vector3 fireDirection)
    {
        if(decalPool == null)
        {
            decalPool = new ObjectPool<DecalController>(maxAmount);
        }
        foreach(Vector3 dir in fireBehaviour.lastFireDirections)
        {
            RaycastHit hit;
            Physics.Raycast(_weapon.Data.Owner.CamTransform.position, dir, out hit);
            if (!EnemyManager.EnemyDict.ContainsKey(hit.collider.gameObject.name))
            {
                if (hit.collider != null)
                {
                    DecalController decal = decalPool.RequestObject();
                    if (decal.decal != null)
                    {
                        decal.decal.transform.SetPositionAndRotation(hit.point + hit.normal * 0.001f, Quaternion.Euler(hit.normal));
                    }
                    else
                    {
                        decal.decal = Instantiate(decalPrefab, hit.point + hit.normal * 0.01f, Quaternion.Euler(hit.normal));
                    }
                }
            }
        }
        return null;
    }
}
