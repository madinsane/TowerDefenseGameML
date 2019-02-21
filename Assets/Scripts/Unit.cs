using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Unit
{
    public float Health { get; set; }

    [Header("Shop")]
    public GameObject prefab;
    public int cost;
    public int defenseUnit;

    [Header("Offense")]
    public bool isMelee = true;
    public float damage = 10;
    public float range = 0.5f;
    public int attackRate = 60;
    public bool isSuicide = false;
    public float areaOfEffect = 0f;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Defense")]
    public float maxHealth = 100;

    [Header("Movement")]
    public float speed = 10f;

    [Header("Misc")]
    public int value = 50;
    public Image healthBar;
    public float aggroRange = 2f;
    public GameObject impactEffect;

    public void InitHealth()
    {
        Health = maxHealth;
    }
}
