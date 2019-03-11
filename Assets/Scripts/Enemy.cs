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
    public int WaveNumber { get; set; } = 1;
    public bool PartOfWave { get; set; }
    public Transform TauntTarget { get; set; }
    public bool PlayerOwned { get; set; }

    public NavMeshAgent agent;
    public NavMeshSurface2d navMesh;

    new void Start()
    {
        base.Start();
        agent.updateRotation = false;
        opponentTag = "Structure";
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (unit.isFlying == true)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        TauntTarget = null;
        PickCore();
        WaveScale();
        attackCountdown = unit.attackRate / 60f;
        //navMesh.BuildNavMesh();
    }

    void PickCore()
    {
        System.Random random = new System.Random();
        if (PlayerOwned)
        {
            int core = random.Next(0, Waypoint.points.Count);
            targetWP = Waypoint.points[core];
        } else
        {
            int core = random.Next(0, Waypoint.opponentPoints.Count);
            targetWP = Waypoint.opponentPoints[core];
        } 
    }

    void WaveScale()
    {
        unit.maxHealth *= (1f + (0.2f * (WaveNumber - 1)));
        unit.damage *= (1f + (0.2f * (WaveNumber - 1)));
        unit.InitHealth();
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
                attackCountdown = unit.attackRate / 60f;
                hasAttacked = true;
            }
            attackCountdown -= Time.deltaTime;
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
        if (PartOfWave)
        {
            if (PlayerOwned)
            {
                WaveSpawner.EnemiesAlive--;
            }
            else
            {
                WaveSpawner.OpponentEnemiesAlive--;
            }
        }
        if (PlayerOwned)
        {
            StatManager.Gold += unit.baseValue;
            StatManager.Score += unit.baseValue;
            StatManager.Food += unit.baseValue / 5;
        } else
        {
            StatManager.OpponentGold += unit.baseValue;
            StatManager.OpponentScore += unit.baseValue;
            StatManager.OpponentFood += unit.baseValue / 5;
        }
        Destroy(gameObject);
    }

    void Move()
    {
        agent.SetDestination(target.transform.position);

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            //transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            Vector3 dir = agent.velocity.normalized;
            float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion toRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 2f);
        }
        agent.speed = unit.speed;
    }

}
