using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Structure
{
    [Header("Shop")]
    public GameObject prefab;
    public int cost;
    public int defenseUnit;

    public float Health { get; set; }

    [Header("Defense")]
    public float maxHealth;

    [Header("Misc")]
    public int value = 50;
    public Image healthBar;
    public float range = 5f;

    public void InitHealth()
    {
        Health = maxHealth;
    }
}
