using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : AttackEntity
{
    private Transform targetWP;
    private int wavepointIndex = 0;
    private float attackCountdown = 0f;
    private bool hasAttacked = false;
    public Transform TauntTarget { get; set; }

    public NavMeshAgent agent;
    public NavMeshSurface2d navMesh;

    new void Start()
    {
        base.Start();
        opponentTag = "Structure";
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (unit.isFlying == true)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        TauntTarget = null;
        PickCore();
        attackCountdown = unit.attackRate;
        //navMesh.BuildNavMesh();
    }

    void PickCore()
    {
        System.Random random = new System.Random();
        int core = random.Next(0, Waypoint.points.Count);
        targetWP = Waypoint.points[core];
    }

    void Update()
    {
        if (targetWP == null)
        {
            return;
        }
        if (TauntTarget == null)
        {
            Aggro();
        } else
        {
            target = TauntTarget;
        }
        agent.isStopped = false;
        if (target == null)
        {
            target = targetWP;
        }
        if (Vector3.SqrMagnitude(transform.position - target.position) <= Mathf.Abs(Mathf.Pow(unit.range,2)))
        {
            agent.isStopped = true;
            //GetNextWaypoint();
            if (attackCountdown <= 0)
            {
                Attack();
                attackCountdown = unit.attackRate;
                hasAttacked = true;
            }
            attackCountdown -= 1;
        }
        if (!hasAttacked)
        {
            //Vector3 dir = target.position - transform.position;
            //transform.Translate(dir.normalized * unit.speed * Time.deltaTime, Space.World);
            Move();
        } else
        {
            hasAttacked = false;
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoint.points.Count - 1)
        {
            Destroy(gameObject);
            return;
        }
        wavepointIndex++;
        targetWP = Waypoint.points[wavepointIndex];
    }

    public override void Kill()
    {
        StatManager.Gold += unit.baseValue;
        StatManager.Food += unit.baseValue / 5;
        Destroy(gameObject);
    }

    void Move()
    {
        agent.SetDestination(target.transform.position);
        agent.speed = unit.speed;
    }
}
