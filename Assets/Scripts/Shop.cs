using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    private void Awake()
    {
        buildManager = BuildManager.instance;
    }

    private void Start()
    {
        buildManager = BuildManager.instance;
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
            case "Flameblast":
                Debug.Log("Flameblast Tower selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.flameblast);
                break;
            case "Frostbeam":
                Debug.Log("Frostbeam Tower selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.frostbeam);
                break;
            case "BaseWall":
                Debug.Log("Wall selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.baseWall);
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
            case "Flameblast":
                cost = buildManager.flameblast.cost;
                break;
            case "Frostbeam":
                cost = buildManager.frostbeam.cost;
                break;
            case "BaseWall":
                cost = buildManager.baseWall.cost;
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
            case "Flameblast":
                duCost = buildManager.flameblast.defenseUnit;
                break;
            case "Frostbeam":
                duCost = buildManager.frostbeam.defenseUnit;
                break;
            case "BaseWall":
                duCost = buildManager.baseWall.defenseUnit;
                break;
            default:
                Debug.Log("Structure not found");
                break;
        }
        return duCost;
    }

    public int GetStructureUpgradeCost(Unit structure)
    {
        if (structure.StatusAffected)
        {
            return -2;
        }
        if (structure.UpgradeLevel >= 5)
        {
            return -1;
        }
        int cost = Mathf.FloorToInt(structure.Value * 1.5f);
        //structure.Value = cost + structure.Value;
        return cost;
    }

    public int GetStructureRepairCost(Unit structure)
    {
        float lifePercent = (structure.maxHealth - structure.Health) / structure.maxHealth;
        int cost = Mathf.FloorToInt(structure.Value * lifePercent);
        //structure.Value = cost + structure.Value;
        return cost;
    }

    public int GetStructureSellAmount(Unit structure)
    {
        float lifePercent = (structure.maxHealth - structure.Health) / structure.maxHealth;
        int cost = Mathf.FloorToInt(structure.Value * (1-lifePercent));
        return cost;
    }

    public void RepairStructure(Entity structure)
    {
        int cost = GetStructureRepairCost(structure.unit);
        if (StatManager.Gold >= cost)
        {
            structure.unit.InitHealth();
            StatManager.Gold -= cost;
            StatManager.Score += cost;
        } else
        {
            Debug.Log("Insufficient Gold");
        }
    }

    public void UpgradeStructure(Entity structure)
    {
        int cost = GetStructureUpgradeCost(structure.unit);
        if (cost == -2)
        {
            Debug.Log("Status Affected");
            return;
        }
        if (StatManager.Gold >= cost)
        {
            if (structure.unit.UpgradeLevel < 5)
            {
                structure.unit.UpgradeStructure();
                StatManager.Gold -= cost;
                StatManager.Score += cost;
            } else
            {
                Debug.Log("Already at max level");
            }
        }
        else
        {
            Debug.Log("Insufficient Gold");
        }
    }

    public void SellStructure(Entity structure)
    {
        int cost = GetStructureRepairCost(structure.unit);
        StatManager.Gold += cost;
        structure.Kill();
    }

    public int GetUnitCost(string name)
    {
        int cost = 0;
        switch (name)
        {
            case "Enemy":
                cost = buildManager.baseUnit.cost;
                break;
            default:
                Debug.Log("Unit not found");
                break;
        }
        return cost;
    }

    public void SelectSummon(string name)
    {
        switch (name)
        {
            case "Enemy":
                Debug.Log("Enemy selected");
                buildManager.SummonUnit(buildManager.baseUnit);
                break;
            default:
                Debug.Log("Unit not found");
                break;
        }

    }
}
