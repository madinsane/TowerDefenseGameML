using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEntity : Entity
{
    protected Transform target;

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
            target.gameObject.GetComponent<Entity>().Damage(unit.damage);
            if (unit.isSuicide)
            {
                Kill();
            }
        }
    }

    public override void Kill()
    {
        StatManager.Gold += unit.value;
        Destroy(gameObject);
    }
}
