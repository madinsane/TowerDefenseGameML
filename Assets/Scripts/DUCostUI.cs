using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DUCostUI : MonoBehaviour
{
    public Text duCostText;
    public GameObject shopPrefab;
    public string structureName;

    // Start is called before the first frame update
    void Start()
    {
        int ducost = shopPrefab.GetComponent<Shop>().GetStructureDUCost(structureName);
        duCostText.text = ducost.ToString() + "DU";
    }

}
