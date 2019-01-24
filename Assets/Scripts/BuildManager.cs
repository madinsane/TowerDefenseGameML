using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private Structure turretToBuild;
    private SpriteRenderer spriteRenderer;
    public GameObject placementPrefab;

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

    public Structure GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(Structure turret)
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
                return;
            } else if (StatManager.DefenseUnit + turretToBuild.defenseUnit > StatManager.DefenseUnitMax)
            {
                Debug.Log("Not enough defense units");
                return;
            }
            StatManager.Gold -= turretToBuild.cost;
            StatManager.DefenseUnit += turretToBuild.defenseUnit;
            DisablePreview();
            Instantiate(turretToBuild.prefab, position, Quaternion.identity);
        }
    }

    public void ColorPreview(bool canBuild)
    {
        if (canBuild)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;
        AlphaPreview();
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
