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
    public static int OpponentDefenseUnitMax;
    public static int OpponentScore;
    public int startGold = 500;
    public int startFood = 100;
    public int goldPerTick = 5;
    public int foodPerTick = 1;
    public int defenseUnitMax = 80;

    // Start is called before the first frame update
    void Start()
    {
        Gold = startGold;
        OpponentGold = startGold;
        Food = startFood;
        OpponentFood = startFood;
        DefenseUnit = 0;
        OpponentDefenseUnit = 0;
        DefenseUnitMax = defenseUnitMax;
        OpponentDefenseUnitMax = defenseUnitMax;
        Score = 0;
        OpponentScore = 0;
        InvokeRepeating("GainPerTick", 0f, 1f);
    }

    private void GainPerTick()
    {
        Gold += goldPerTick;
        OpponentGold += goldPerTick;
        Food += foodPerTick;
        OpponentFood += foodPerTick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
