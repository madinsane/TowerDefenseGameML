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
}
