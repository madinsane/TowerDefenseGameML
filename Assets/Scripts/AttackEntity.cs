using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEntity : Entity
{
    protected Transform target;
    public string opponentTag;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Aggro()
    {
        bool foundTarget = false;
        float minDistance = float.MaxValue;
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, unit.aggroRange);
        if (hit == null)
        {
            target = null;
            return;
        }
        for (int i = 0; i < hit.Length; i++)
        {
            string targetTag = "Default";
            if (gameObject.tag == "Enemy")
            {
                targetTag = "Structure";
            } else if (gameObject.tag == "Structure")
            {
                targetTag = "Enemy";
            }
            if (hit[i].tag.Equals(targetTag))
            {
                Vector3 curPosition = hit[i].transform.position;
                float curDistance = Vector3.SqrMagnitude(transform.position - curPosition);
                if (curDistance < minDistance)
                {
                    target = hit[i].transform;
                    minDistance = curDistance;
                    foundTarget = true;
                }
            }
        }
        if (!foundTarget)
        {
            target = null;
        }
    }

    protected void Attack()
    {
        if (target != null)
        {
            if (unit.isMelee)
            {
                if (unit.areaOfEffect > 0f)
                {
                    Explode(transform);
                } else
                {
                    CreateDamage(target.gameObject.GetComponent<Entity>());
                }
                GameObject effect = Instantiate(unit.impactEffect, target.position, transform.rotation);
                effect.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.z - 90, 0, 0);
            } else
            {
                Shoot();
            }
            if (unit.isSuicide)
            {
                Kill();
            }
        }
    }

    protected void Shoot()
    {
        GameObject projectileNew = Instantiate(unit.bulletPrefab, unit.firePoint.position, unit.firePoint.rotation);
        Projectile projectile = projectileNew.GetComponent<Projectile>();
        projectile.OpponentTag = opponentTag;
        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }

    protected void Explode(Transform transform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, unit.areaOfEffect);
        foreach (Collider2D collider in colliders)
        {
            if (tag == "Enemy")
            {
                if (collider.tag == "CoreEntity")
                {
                    Entity entity = collider.GetComponent<Entity>();
                    CreateDamage(entity);
                }
            }
            if (collider.tag == opponentTag)
            {
                Entity entity = collider.GetComponent<Entity>();
                CreateDamage(entity);
            }
        }
    }

    protected void CreateDamage(Entity entity)
    {
        if (unit.isPureStatus)
        {
            entity.TakeStatus(Status.ApplyStatus(unit.damageType, entity.unit.statusResist));
        } else if (unit.isPureDOT)
        {
            CreateDOT(entity);
        } else
        {
            float defense = Damage.CalculateDefense(unit.damageType, unit.damageSource, entity.unit.resists);
            Damage.Packet packet = Damage.CalculateDamage(unit.damageType, unit.damage, defense, unit.critChance, unit.statusChance, entity.unit.statusResist, entity.unit.maxHealth);
            if (packet.isStatus)
            {
                entity.TakeStatus(packet.status);
            }
            if (entity.unit.canReflect)
            {
                CreateDamage(this);
            }
            entity.TakeDamage(packet.damage);
        }
    }

    protected void CreateDOT(Entity entity)
    {
        float defense = Damage.CalculateDefense(unit.damageType, unit.damageSource, entity.unit.resists);
        Damage.DOT dot = Damage.CalculateDot(unit.damageType, unit.damage, defense, entity.unit.maxHealth);
        entity.TakeDOT(dot);
    }

    public override void Kill()
    {
        StatManager.Gold += unit.baseValue;
        Destroy(gameObject);
    }
}
