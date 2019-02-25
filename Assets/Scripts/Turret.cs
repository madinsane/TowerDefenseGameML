using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AttackEntity
{
    private Enemy enemy;

    private LineRenderer rangeLine;

    [Header("Bullet Attributes")]
    public float range = 5f;
    public int fireRate = 60;
    private int fireCountdown = 0;

    [Header("Laser Attributes")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public float laserDamage = 1f;
    public ParticleSystem laserEffect;

    [Header("Setup")]
    public float rotationSpeed = 1.75f;
    public GameObject rangeIndicator;
    public Material rangeMaterial;
    public GameObject optionCanvasPrefab;

    //public GameObject bulletPrefab;
    //public Transform firePoint;
    public bool IsSelected { get; set; }
    private bool mouseUpSelect = false;

    // Start is called before the first frame update
    new void Start()
    {
        unit.InitHealth();
        unit.InitStructure();
        opponentTag = "Enemy";
        fireCountdown = fireRate;
        if (useLaser)
        {
            lineRenderer.enabled = false;
            laserEffect.Stop();
        }
        rangeLine = rangeIndicator.DrawCircle(range/2, 0.1f, rangeMaterial);
        rangeLine.enabled = false;
        optionCanvasPrefab.SetActive(false);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(opponentTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
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
    void Update()
    {
        if (mouseUpSelect && IsSelected && Input.GetMouseButtonDown(0))
        {
            IsSelected = false;
            mouseUpSelect = false;
            rangeLine.enabled = false;
            optionCanvasPrefab.SetActive(false);
        }

        if (IsSelected)
        {
            if (BuildManager.instance.IsPlacing)
            {
                IsSelected = false;
                mouseUpSelect = false;
                rangeLine.enabled = false;
                optionCanvasPrefab.SetActive(false);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseUpSelect = true;
            }
            DisplayStats();
        }
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
            fireCountdown = fireRate;
        }
        fireCountdown -= 1;
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
        enemy.Damage(laserDamage);
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

    void DisplayStats()
    {
        if (!rangeLine.enabled)
        {
            rangeLine.enabled = true;
            optionCanvasPrefab.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
