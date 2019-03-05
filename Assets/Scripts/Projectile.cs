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

    private void Start()
    {
        StartCoroutine(LifeTimeControl());
        if (projectileData.isPiercing)
            StartCoroutine(PierceRepeat());
    }

    IEnumerator LifeTimeControl()
    {
        yield return new WaitForSeconds(projectileData.lifeTime);
        Destroy(gameObject);
    }

    IEnumerator PierceRepeat()
    {
        for (int i = 0; i < projectileData.lifeTime / 0.1f; i++)
        {
            if (TestExplode())
            {
                Explode();
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void Seek (Transform _target)
    {
        target = _target;
        dir = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null && !projectileData.isPiercing)
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
            CreateDamage(target.GetComponent<Entity>());
        }
        if (projectileData.isPiercing)
            return;
        Destroy(gameObject);
    }

    protected void CreateDamage(Entity entity)
    {
        if (projectileData.isPureStatus)
        {
            entity.TakeStatus(Status.ApplyStatus(projectileData.damageType, entity.unit.statusResist));
        }
        else if (projectileData.isPureDOT)
        {
            CreateDOT(entity);
        } else
        {
            float defense = Damage.CalculateDefense(projectileData.damageType, projectileData.damageSource, entity.unit.resists);
            Damage.Packet packet = Damage.CalculateDamage(projectileData.damageType, projectileData.damage, defense, projectileData.critChance, projectileData.statusChance, entity.unit.statusResist, entity.unit.maxHealth);
            if (packet.isStatus)
            {
                entity.TakeStatus(packet.status);
            }
            entity.TakeDamage(packet.damage);
        }
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, projectileData.explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == OpponentTag)
            {
                CreateDamage(collider.GetComponent<Entity>());
            }
        }
    }

    protected void CreateDOT(Entity entity)
    {
        float defense = Damage.CalculateDefense(projectileData.damageType, projectileData.damageSource, entity.unit.resists);
        Damage.DOT dot = Damage.CalculateDot(projectileData.damageType, projectileData.damage, defense, entity.unit.maxHealth);
        entity.TakeDOT(dot);
    }

    bool TestExplode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, projectileData.explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == OpponentTag)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileData.explosionRadius);
    }
}
