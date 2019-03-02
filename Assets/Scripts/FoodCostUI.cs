using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCostUI : MonoBehaviour
{
    public Text costText;
    public GameObject shopPrefab;
    public string unitName;

    // Start is called before the first frame update
    void Start()
    {
        int cost = shopPrefab.GetComponent<Shop>().GetUnitCost(unitName);
        costText.text = cost.ToString() + "f";
    }
}
