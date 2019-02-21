using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;

    private Vector3 dir;

    //[Header("Attributes")]
    //public float speed = 0.03f;
    //public bool homing = true;
    //public float explosionRadius = 0f;
    //public GameObject impactEffect;

    public string OpponentTag { get; set; }

    public ProjectileData projectileData;

    public void Seek (Transform _target)
    {
        target = _target;
        dir = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (projectileData.homing)
            dir = target.position - transform.position;
        if (dir.magnitude <= projectileData.speed)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * projectileData.speed, Space.World);
        dir.Normalize();
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    void HitTarget()
    {
        GameObject effect = Instantiate(projectileData.impactEffect, transform.position, transform.rotation);
        effect.transform.eulerAngles = new Vector3(-90f, 0, 0);
        if (projectileData.explosionRadius > 0f)
        {
            Explode();
        } else
        {
            Damage(target);
        }
        
        Destroy(gameObject);
    }

    void Damage(Transform _entity)
    {
        Entity entity = _entity.GetComponent<Entity>();
        entity.Damage(projectileData.damage);
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, projectileData.explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == OpponentTag)
            {
                Damage(collider.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileData.explosionRadius);
    }
}
