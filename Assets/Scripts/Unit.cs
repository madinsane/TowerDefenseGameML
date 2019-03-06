using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Unit
{
    public float Health { get; set; }
    public int UpgradeLevel { get; set; }
    public int Value { get; set; }
    public bool StatusAffected { get; set; }

    [Header("Offense")]
    public bool isMelee = true;
    public float damage = 10;
    public float range = 0.5f;
    public int attackRate = 60;
    public bool isSuicide = false;
    public float areaOfEffect = 0f;
    public Damage.Type damageType = Damage.Type.Physical;
    public Damage.Source damageSource = Damage.Source.Melee;
    public int critChance = 0;
    public int statusChance = 0;
    public bool isPureStatus = false;
    public bool isPureDOT = false;
    public bool targetAirOnly = false;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Defense")]
    public float maxHealth = 100;
    public float statusResist = 0f;
    public Damage.Resist resists;
    public float regenPercent = 0f;
    public bool canReflect = false;

    [Header("Movement")]
    public float speed = 10f;
    public bool isFlying = false;

    [Header("Structure")]
    public float damageUpFactor = 0.25f;
    public float healthUpFactor = 0.50f;
    public float attackRateUpFactor = 0.1f;
    public float valueFactor = 0.5f;
    public int defenseUnit = 0;

    [Header("Misc")]
    public int baseValue = 50;
    public Image healthBar;
    public HorizontalLayoutGroup upgradeIcons;
    public float aggroRange = 2f;
    public GameObject impactEffect;
    public string name = "";

    public void InitHealth()
    {
        Health = maxHealth;
        healthBar.fillAmount = (Health / maxHealth);
        StatusAffected = false;
    }

    public void InitStructure()
    {
        Value = baseValue;
        UpgradeLevel = 0;
        UpdateUpgradeIcons();
    }

    public void UpgradeStructure()
    {
        if (StatusAffected)
            return;
        UpgradeLevel++;
        damage *= (1 + damageUpFactor);
        maxHealth *= (1 + healthUpFactor);
        attackRate = Mathf.FloorToInt(attackRate * (1 - attackRateUpFactor));
        Value = Mathf.FloorToInt(Value * (1 + valueFactor));
        InitHealth();
        UpdateUpgradeIcons();
    }

    private void UpdateUpgradeIcons()
    {
        Image[] icons = upgradeIcons.GetComponentsInChildren<Image>();
        for (int i = 0; i < icons.Length; i++)
        {
            if (i < UpgradeLevel)
            {
                icons[i].enabled = true;
            } else
            {
                icons[i].enabled = false;
            }
        }
    }
}
