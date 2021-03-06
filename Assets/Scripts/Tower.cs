﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    private Enemy enemy;

    [Header("Bullet Attributes")]
    public float range = 5f;
    private float fireCountdown = 0;

    [Header("Laser Attributes")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public float laserDamage = 1f;
    public ParticleSystem laserEffect;

    [Header("Setup")]
    public float rotationSpeed = 1.75f;

    //public GameObject bulletPrefab;
    //public Transform firePoint;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        opponentTag = "Enemy";
        fireCountdown = unit.attackRate / 60f;
        if (useLaser)
        {
            lineRenderer.enabled = false;
            laserEffect.Stop();
        }
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(opponentTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            if (unit.targetAirOnly)
            {
                Enemy enemyComp = enemy.GetComponent<Enemy>();
                if (!enemyComp.unit.isFlying)
                {
                    continue;
                }
            }
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            enemy = target.GetComponent<Enemy>();
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    laserEffect.Stop();
                }
            }
            return;
        }
        LockOn();
        if (useLaser)
        {
            Laser();
        }
        else if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = unit.attackRate / 60f;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Laser()
    {
        lineRenderer.SetPosition(0, unit.firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            laserEffect.Play();
        }
        if (unit.areaOfEffect > 0)
        {
            Explode(enemy.transform);
        } else
        {
            CreateDamage(enemy);
        }
        laserEffect.transform.position = target.position;
        Vector3 dir = transform.position - target.position;
        dir.Normalize();
        float rotation_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion toRotation = Quaternion.Euler(0f, 0f, rotation_z - 270);
        laserEffect.transform.rotation = Quaternion.Lerp(laserEffect.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        
    }

    void LockOn()
    {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        float rotation_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion toRotation = Quaternion.Euler(0f, 0f, rotation_z - 270);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
    }

    /*void Shoot()
    {
        GameObject projectileNew = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileNew.GetComponent<Projectile>();
        projectile.OpponentTag = opponentTag;
        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
