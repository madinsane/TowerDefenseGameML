using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodUI : MonoBehaviour
{
    public Text foodText;

    // Update is called once per frame
    void Update()
    {
        foodText.text = StatManager.Food.ToString() + "f";
    }
}
