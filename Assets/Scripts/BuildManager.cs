﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private StructureShopData turretToBuild;
    private SpriteRenderer spriteRenderer;
    [Header("Setup")]
    public GameObject placementPrefab;
    public Transform spawnPoint;
    public Text shopText;
    public Transform barrier;

    [Header("Structures")]
    public StructureShopData ballista;
    public StructureShopData flameblast;
    public StructureShopData frostbeam;
    public StructureShopData lightningSpire;
    public StructureShopData plague;

    public StructureShopData organicWall;
    public StructureShopData moltenWall;
    public StructureShopData mirrorWall;

    [Header("Units")]
    public Summon grunt;
    public Summon wisp;
    public Summon volatileSpiderling;
    public Summon goliath;
    public Summon catapult;
    public Summon drake;

    public bool IsPlacing { get; set; }
    public string CurrentTurret { get; set; }
    public bool CanBuild { get { return turretToBuild != null; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple BuildManagers in scene");
            return;
        }
        instance = this;
        IsPlacing = false;
        //placementPrefab.SetActive(false);
        spriteRenderer = placementPrefab.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        AlphaPreview();
    }

    public StructureShopData GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(StructureShopData turret)
    {
        turretToBuild = turret;
        IsPlacing = true;
        spriteRenderer.sprite = turret.prefab.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.enabled = true;
    }

    public void BuildStructure(Vector3 position)
    {
        if (CanBuild)
        {
            if (StatManager.Gold < turretToBuild.cost)
            {
                Debug.Log("Not enough gold");
                shopText.text = "Not enough gold";
                return;
            } else if (StatManager.DefenseUnit + turretToBuild.defenseUnit > StatManager.DefenseUnitMax)
            {
                Debug.Log("Not enough defense units");
                shopText.text = "Not enough defense units";
                return;
            }
            StatManager.Gold -= turretToBuild.cost;
            StatManager.Score += turretToBuild.cost;
            StatManager.DefenseUnit += turretToBuild.defenseUnit;
            DisablePreview();
            Instantiate(turretToBuild.prefab, position, Quaternion.identity);
        }
    }

    public void SummonUnit(Summon unit)
    {
        if (StatManager.Food < unit.cost)
        {
            Debug.Log("Not enough food");
            shopText.text = "Not enough food";
            return;
        }
        StatManager.Food -= unit.cost;
        StatManager.Score += unit.cost;
        Instantiate(unit.prefab, spawnPoint.position, Quaternion.identity);
    }

    public void ColorPreview(bool canBuild)
    {
        if (canBuild)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;
        AlphaPreview();
        placementPrefab.transform.localScale = new Vector3(turretToBuild.scale, turretToBuild.scale, 0);
    }

    private void AlphaPreview()
    {
        Color tmp = spriteRenderer.color;
        tmp.a = 0.75f;
        spriteRenderer.color = tmp;
        
    }

    public void DisablePreview()
    {
        IsPlacing = false;
        spriteRenderer.enabled = false;
    }
}
