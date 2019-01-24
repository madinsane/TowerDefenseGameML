using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    public Text goldText;

    // Update is called once per frame
    void Update()
    {
        goldText.text = StatManager.Gold.ToString() + "g";
    }
}
