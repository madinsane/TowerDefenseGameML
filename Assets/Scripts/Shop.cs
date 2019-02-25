using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    private void Awake()
    {
        buildManager = BuildManager.instance;
        Debug.Log("Shop started");
    }

    public void SelectStructure(string name)
    {
        switch (name)
        {
            case "Turret":
                Debug.Log("Turret selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.baseTurret);
                break;
            case "MissileTurret":
                Debug.Log("Missile Turret selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.missileTurret);
                break;
            case "LaserTurret":
                Debug.Log("Laser Turret selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.laserTurret);
                break;
            default:
                Debug.Log("Structure not found");
                break;
        }
        
    }

    public int GetStructureCost(string name)
    {
        int cost = 0;
        switch (name)
        {
            case "Turret":
                cost = buildManager.baseTurret.cost;
                break;
            case "MissileTurret":
                cost = buildManager.missileTurret.cost;
                break;
            case "LaserTurret":
                cost = buildManager.laserTurret.cost;
                break;
            default:
                Debug.Log("Structure not found");
                break;
        }
        return cost;
    }

    public int GetStructureDUCost(string name)
    {
        int duCost = 0;
        switch (name)
        {
            case "Turret":
                duCost = buildManager.baseTurret.defenseUnit;
                break;
            case "MissileTurret":
                duCost = buildManager.missileTurret.defenseUnit;
                break;
            case "LaserTurret":
                duCost = buildManager.laserTurret.defenseUnit;
                break;
            default:
                Debug.Log("Structure not found");
                break;
        }
        return duCost;
    }

    public int GetStructureUpgradeCost(Unit structure)
    {
        int cost = Mathf.FloorToInt(structure.Value * 1.5f);
        //structure.Value = cost + structure.Value;
        return cost;
    }

    public int GetStructureRepairCost(Unit structure)
    {
        float lifePercent = (structure.maxHealth - structure.Health) / structure.maxHealth;
        int cost = Mathf.FloorToInt(structure.Value * lifePercent * 0.5f);
        //structure.Value = cost + structure.Value;
        return cost;
    }

    public int GetStructureSellAmount(Unit structure)
    {
        return structure.Value;
    }
}
