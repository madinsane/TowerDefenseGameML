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
            case "Ballista":
                Debug.Log("Ballista selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.ballista);
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
            case "LightningSpire":
                Debug.Log("Lightning Spire selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.lightningSpire);
                break;
            case "Plague":
                Debug.Log("Plague Tower selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.plague);
                break;
            case "OrganicWall":
                Debug.Log("Organic Wall selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.organicWall);
                break;
            case "MoltenWall":
                Debug.Log("Molten Wall selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.moltenWall);
                break;
            case "MirrorWall":
                Debug.Log("Mirror Wall selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(buildManager.mirrorWall);
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
            case "Ballista":
                cost = buildManager.ballista.cost;
                break;
            case "Flameblast":
                cost = buildManager.flameblast.cost;
                break;
            case "Frostbeam":
                cost = buildManager.frostbeam.cost;
                break;
            case "LightningSpire":
                cost = buildManager.lightningSpire.cost;
                break;
            case "Plague":
                cost = buildManager.plague.cost;
                break;
            case "OrganicWall":
                cost = buildManager.organicWall.cost;
                break;
            case "MoltenWall":
                cost = buildManager.moltenWall.cost;
                break;
            case "MirrorWall":
                cost = buildManager.mirrorWall.cost;
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
            case "Ballista":
                duCost = buildManager.ballista.defenseUnit;
                break;
            case "Flameblast":
                duCost = buildManager.flameblast.defenseUnit;
                break;
            case "Frostbeam":
                duCost = buildManager.frostbeam.defenseUnit;
                break;
            case "LightningSpire":
                duCost = buildManager.lightningSpire.defenseUnit;
                break;
            case "Plague":
                duCost = buildManager.plague.defenseUnit;
                break;
            case "OrganicWall":
                duCost = buildManager.organicWall.defenseUnit;
                break;
            case "MoltenWall":
                duCost = buildManager.moltenWall.defenseUnit;
                break;
            case "MirrorWall":
                duCost = buildManager.mirrorWall.defenseUnit;
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
            buildManager.shopText.text = "Insufficient Gold";
        }
    }

    public int RepairStructureOpp(Entity structure)
    {
        int cost = GetStructureRepairCost(structure.unit);
        if (StatManager.OpponentGold >= cost)
        {
            structure.unit.InitHealth();
            StatManager.OpponentGold -= cost;
            StatManager.OpponentScore += cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public void UpgradeStructure(Entity structure)
    {
        int cost = GetStructureUpgradeCost(structure.unit);
        if (cost == -2)
        {
            Debug.Log("Status Affected");
            buildManager.shopText.text = "Status Affected";
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
                buildManager.shopText.text = "Already at max level";
            }
        }
        else
        {
            Debug.Log("Insufficient Gold");
            buildManager.shopText.text = "Insufficient Gold";
        }
    }

    public int UpgradeStructureOpp(Entity structure)
    {
        int cost = GetStructureUpgradeCost(structure.unit);
        if (cost == -2)
        {
            return 1;
        }
        if (StatManager.OpponentGold >= cost)
        {
            if (structure.unit.UpgradeLevel < 5)
            {
                structure.unit.UpgradeStructure();
                StatManager.OpponentGold -= cost;
                StatManager.OpponentScore += cost;
                return 0;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            return 3;
        }
    }

    public void SellStructure(Entity structure)
    {
        int cost = GetStructureSellAmount(structure.unit);
        StatManager.Gold += cost;
        structure.Kill();
    }

    public void SellStructureOpp(Entity structure)
    {
        int cost = GetStructureSellAmount(structure.unit);
        StatManager.OpponentGold += cost;
        structure.Kill();
    }

    public int GetUnitCost(string name)
    {
        int cost = 0;
        switch (name)
        {
            case "Grunt":
                cost = buildManager.grunt.cost;
                break;
            case "Wisp":
                cost = buildManager.wisp.cost;
                break;
            case "Volatile":
                cost = buildManager.volatileSpiderling.cost;
                break;
            case "Catapult":
                cost = buildManager.catapult.cost;
                break;
            case "Drake":
                cost = buildManager.drake.cost;
                break;
            case "Goliath":
                cost = buildManager.goliath.cost;
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
            case "Grunt":
                Debug.Log("Grunt selected");
                buildManager.SummonUnit(buildManager.grunt);
                break;
            case "Wisp":
                Debug.Log("Wisp selected");
                buildManager.SummonUnit(buildManager.wisp);
                break;
            case "Volatile":
                Debug.Log("Volatile Spiderling selected");
                buildManager.SummonUnit(buildManager.volatileSpiderling);
                break;
            case "Catapult":
                Debug.Log("Catapult selected");
                buildManager.SummonUnit(buildManager.catapult);
                break;
            case "Drake":
                Debug.Log("Drake selected");
                buildManager.SummonUnit(buildManager.drake);
                break;
            case "Goliath":
                Debug.Log("Goliath selected");
                buildManager.SummonUnit(buildManager.goliath);
                break;
            default:
                Debug.Log("Unit not found");
                break;
        }

    }
}
