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
    public int startGold = 500;
    public int startFood = 100;
    public int goldPerTick = 5;
    public int foodPerTick = 1;
    public int defenseUnitMax = 80;

    // Start is called before the first frame update
    void Start()
    {
        Gold = startGold;
        Food = startFood;
        DefenseUnit = 0;
        DefenseUnitMax = defenseUnitMax;
        Score = 0;
        InvokeRepeating("GainPerTick", 0f, 1f);
    }

    private void GainPerTick()
    {
        Gold += goldPerTick;
        Food += foodPerTick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
