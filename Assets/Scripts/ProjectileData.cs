using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData
{
    public float damage;
    public float speed = 0.03f;
    public bool homing = true;
    public float explosionRadius = 0f;
    public GameObject impactEffect;

    public Damage.Type damageType = Damage.Type.Physical;
    public Damage.Source damageSource = Damage.Source.Ranged;
    public int critChance = 0;
    public int statusChance = 0;
    public bool isPureStatus = false;

    public bool isPiercing = false;
    public float lifeTime = 1f;
}
