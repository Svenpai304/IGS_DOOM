using UnityEngine;

public class GrenadeController
{
    private GameObject projectile;
    private Transform rbTransform;
    private GameObject explodeEffect;
    private SingleGrenade.GrenadeData data;

    public GrenadeController(GameObject _projectile, Vector3 startForce, SingleGrenade.GrenadeData _data)
    {
        GameManager.GlobalFixedUpdate += FixedUpdate;
        projectile = _projectile;
        var rb = projectile.GetComponentInChildren<Rigidbody>();
        rbTransform = rb.transform;
        data = _data;
        projectile.GetComponentInChildren<Rigidbody>().AddForce(startForce);
    }

    private void FixedUpdate()
    {
        if (projectile != null)
        {
            Debug.DrawLine(rbTransform.position, rbTransform.position + new Vector3(data.hitRadius, 0, 0), Color.green, 0.1f);
            if (Physics.CheckSphere(rbTransform.position, data.hitRadius, data.hitMask))
            {
                Debug.Log("Exploding");
                Explode();
            }
        }
        else
        {
            data.explodeEffectDuration -= Time.fixedDeltaTime;
            if(data.explodeEffectDuration < 0 && explodeEffect != null)
            {
                Object.Destroy(explodeEffect);
                GameManager.GlobalFixedUpdate -= FixedUpdate;
            }
        }

    }

    private void Explode()
    {
        explodeEffect = Object.Instantiate(data.explosionPrefab, rbTransform.position, rbTransform.rotation);
        Object.Destroy(projectile);
        Collider[] colliders = Physics.OverlapSphere(rbTransform.position, data.explodeRadius.GetValue(), LayerMask.GetMask("Damageable"));
        foreach (Collider collider in colliders)
        {
            if (EnemyManager.EnemyDict.ContainsKey(collider.name))
            {
                EnemyManager.EnemyDict[collider.name].TakeDamage(data.damage);
            }
        }
    }
}
