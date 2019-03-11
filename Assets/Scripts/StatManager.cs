using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static int Gold;
    public static int Food;
    public static int DefenseUnit;
    public static int DefenseUnitMax;
    public static int Score;
    public static int OpponentGold;
    public static int OpponentFood;
    public static int OpponentDefenseUnit;
    public static int OpponentScore;

    public enum VictoryState
    {
        None, Player, CPU
    };
    public static VictoryState victoryState;

    public int startGold = 500;
    public int startFood = 100;
    public int goldPerTick = 5;
    public int foodPerTick = 1;
    public int defenseUnitMax = 80;
    public int startScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        victoryState = VictoryState.None;
        Gold = startGold;
        OpponentGold = startGold;
        Food = startFood;
        OpponentFood = startFood;
        DefenseUnit = 0;
        OpponentDefenseUnit = 0;
        DefenseUnitMax = defenseUnitMax;
        Score = 0;
        OpponentScore = 0;
        InvokeRepeating("GainPerTick", 0f, 1f);
    }

    public void Restart()
    {
        victoryState = VictoryState.None;
        Gold = startGold;
        OpponentGold = startGold;
        Food = startFood;
        OpponentFood = startFood;
        DefenseUnit = 0;
        OpponentDefenseUnit = 0;
        DefenseUnitMax = defenseUnitMax;
        Score = 0;
        OpponentScore = 0;
    }

    private void GainPerTick()
    {
        Gold += goldPerTick;
        OpponentGold += goldPerTick;
        Food += foodPerTick;
        OpponentFood += foodPerTick;
        if (victoryState != VictoryState.None)
        {
            if (victoryState == VictoryState.Player)
            {
                Debug.Log("You Win!");
            } else
            {
                Debug.Log("CPU Wins!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
