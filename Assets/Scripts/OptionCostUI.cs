using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionCostUI : MonoBehaviour
{
    public Text costText;
    public GameObject shopPrefab;
    public GameObject structurePrefab;
    public enum OptionType
    {
        Upgrade, Sell, Repair
    };
    public OptionType option;

    private Shop shop;
    private Unit unit;
    private int cost;

    // Start is called before the first frame update
    void Start()
    {
        shop = shopPrefab.GetComponent<Shop>();
        unit = structurePrefab.GetComponent<Turret>().unit;
        InvokeRepeating("UpdateCost", 0f, 0.5f);
    }

    void UpdateCost()
    {
        switch (option)
        {
            case OptionType.Upgrade:
                cost = shop.GetStructureUpgradeCost(unit);
                break;
            case OptionType.Repair:
                cost = shop.GetStructureRepairCost(unit);
                break;
            case OptionType.Sell:
                cost = shop.GetStructureSellAmount(unit);
                break;
        }
        costText.text = cost.ToString() + "g";
    }
}
