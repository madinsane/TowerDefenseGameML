using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostUI : MonoBehaviour
{
    public Text costText;
    public GameObject shopPrefab;
    public string structureName;

    // Start is called before the first frame update
    void Start()
    {
        int cost = shopPrefab.GetComponent<Shop>().GetStructureCost(structureName);
        costText.text = cost.ToString() + "g";
    }

}
