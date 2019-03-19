using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class BuildAgent : Agent
{
    [Header("Setup")]
    public GameObject buildLocationParent;
    public Shop shop;

    private Transform[] buildLocations;
    private bool[] canBuild;
    private Structure[] structures;
    private BuildManager bm;

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> tempLocations = new List<Transform>(buildLocationParent.GetComponentsInChildren<Transform>());
        tempLocations.Remove(buildLocationParent.transform);
        buildLocations = tempLocations.ToArray();
        canBuild = new bool[21];
        for (int i = 0; i < canBuild.Length; i++)
        {
            canBuild[i] = true;
        }
        structures = new Structure[21];
        bm = BuildManager.instance;
    }

    private bool CheckLocation(int location)
    {
        if (!canBuild[location])
        {
            if (structures[location] == null)
            {
                SetReward(-0.3f);
            }
        }
        canBuild[location] = (structures[location] == null);
        return canBuild[location];
    }

    private void SetLocation(int location)
    {
        canBuild[location] = false;
    }

    private void BuildLocation(int location, StructureShopData structure)
    {
        if (CheckLocation(location))
        {
            (Structure newStructure, int response) = bm.BuildStructureOpp(buildLocations[location].position, structure);
            Debug.Log("Build: " + location + structure);
            if (response == 0)
            {
                SetLocation(location);
                structures[location] = newStructure;
                SetReward(1f);
            }
        } else
        {
            //SetReward(-0.1f);
        }
    }

    private void UpgradeLocation(int location)
    {
        if (!CheckLocation(location))
        {
            int response = shop.UpgradeStructureOpp(structures[location]);
            Debug.Log("Upgrade: " + location);
            if (response == 0)
            {
                SetReward(0.3f);
            }
        } else
        {
            //SetReward(-0.1f);
        }
    }

    private void SellLocation(int location)
    {
        if (!CheckLocation(location))
        {
            shop.SellStructureOpp(structures[location]);
            Debug.Log("Sold: " + location);
        }
        else
        {
            //SetReward(-0.1f);
        }
    }

    private void RepairLocation(int location)
    {
        if (!CheckLocation(location))
        {
            int response = shop.RepairStructureOpp(structures[location]);
            Debug.Log("Repair: " + location);
            if (response == 0)
            {
                //SetReward(0.01f);
            }
        }
        else
        {
            //SetReward(-0.1f);
        }
    }

    private void SummonUnit(Summon summon)
    {
        int response = bm.SummonUnitOpp(summon);
        Debug.Log("Summon: " + summon);
        if (response == 0)
        {
            SetReward(summon.cost/1000f);
        }
    }

    private float CreateReward()
    {
        float regret = Mathf.Abs(StatManager.Score - StatManager.OpponentScore);
        float maxValue = 10000000f;
        float normalRegret = (regret - 0) / (maxValue - 0);
        return (1 - normalRegret) / 1000f;
    }

    //public void AgentReset()

    public override void CollectObservations()
    {
        //AddVectorObs(StatManager.OpponentGold);
        AddVectorObs(StatManager.OpponentFood);
        //AddVectorObs(StatManager.OpponentDefenseUnit);
        //AddVectorObs(StatManager.DefenseUnitMax);
        AddVectorObs(StatManager.Score);
        AddVectorObs(StatManager.OpponentScore);
        //foreach (bool build in canBuild)
        //{
        //    AddVectorObs(build);
        //}
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        SetReward(CreateReward());
        //Debug.Log("Action: " + textAction);
        //int action = Mathf.FloorToInt(vectorAction[0]);
        //switch (action)
        //{
        //    case 0:
        //        // do nothing
        //        break;
        //    case 1:
        //        // Molten Top
        //        BuildLocation(0, bm.moltenWall);
        //        break;
        //    case 2:
        //        //Molten Bot
        //        BuildLocation(1, bm.moltenWall);
        //        break;
        //    case 3:
        //        //Organic Top
        //        BuildLocation(0, bm.organicWall);
        //        break;
        //    case 4:
        //        //Organic Bot
        //        BuildLocation(1, bm.organicWall);
        //        break;
        //    case 5:
        //        //Mirror Top
        //        BuildLocation(0, bm.mirrorWall);
        //        break;
        //    case 6:
        //        //Mirror Bot
        //        BuildLocation(1, bm.mirrorWall);
        //        break;
        //    default:
        //        throw new ArgumentException("Invalid action value");
        //}
        ////Build Tower
        //action = Mathf.FloorToInt(vectorAction[1]);
        //StructureShopData structure;
        //int offset;
        //if (action != 0)
        //{
        //    if (action <= 18)
        //    {
        //        structure = bm.ballista;
        //        offset = -2;
        //    }
        //    else if (action <= 36)
        //    {
        //        structure = bm.flameblast;
        //        offset = 16;
        //    }
        //    else if (action <= 54)
        //    {
        //        structure = bm.frostbeam;
        //        offset = 34;
        //    }
        //    else
        //    {
        //        structure = bm.plague;
        //        offset = 52;
        //    }
        //    BuildLocation(action - offset, structure);
        //}
        ////Build AA
        //action = Mathf.FloorToInt(vectorAction[2]);
        //if (action != 0)
        //{
        //    BuildLocation(2, bm.lightningSpire);
        //}
        ////Upgrade 
        //action = Mathf.FloorToInt(vectorAction[3]);
        //if (action != 0)
        //{
        //    UpgradeLocation(action - 1);
        //}
        ////Sell
        //action = Mathf.FloorToInt(vectorAction[4]);
        //if (action != 0)
        //{
        //    SellLocation(action - 1);
        //}
        ////Repair
        //action = Mathf.FloorToInt(vectorAction[5]);
        //if (action != 0)
        //{
        //    RepairLocation(action - 1);
        //}
        //Summon
        int action = Mathf.FloorToInt(vectorAction[0]);
        switch (action)
        {
            case 0:
                //Do nothing
                break;
            case 1:
                SummonUnit(bm.grunt);
                break;
            case 2:
                SummonUnit(bm.wisp);
                break;
            case 3:
                SummonUnit(bm.volatileSpiderling);
                break;
            case 4:
                SummonUnit(bm.drake);
                break;
            case 5:
                SummonUnit(bm.catapult);
                break;
            case 6:
                SummonUnit(bm.goliath);
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }

        if (StatManager.victoryState != StatManager.VictoryState.None)
        {
            if (StatManager.victoryState == StatManager.VictoryState.Player)
            {
                AddReward(-1f);
                Done();
            }
            else
            {
                Done();
            }
        }
    }
}
