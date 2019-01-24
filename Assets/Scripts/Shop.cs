using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public Structure baseTurret;
    public Structure missileTurret;
    public Structure laserTurret;

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
                buildManager.SetTurretToBuild(baseTurret);
                break;
            case "MissileTurret":
                Debug.Log("Missile Turret selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(missileTurret);
                break;
            case "LaserTurret":
                Debug.Log("Missile Turret selected");
                buildManager.CurrentTurret = name;
                buildManager.SetTurretToBuild(laserTurret);
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
                cost = baseTurret.cost;
                break;
            case "MissileTurret":
                cost = missileTurret.cost;
                break;
            case "LaserTurret":
                cost = laserTurret.cost;
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
                duCost = baseTurret.defenseUnit;
                break;
            case "MissileTurret":
                duCost = missileTurret.defenseUnit;
                break;
            case "LaserTurret":
                duCost = laserTurret.defenseUnit;
                break;
            default:
                Debug.Log("Structure not found");
                break;
        }
        return duCost;
    }
}
