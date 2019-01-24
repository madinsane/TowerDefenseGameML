using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DUUI : MonoBehaviour
{
    public Text defenseUnitText;

    // Update is called once per frame
    void Update()
    {
        defenseUnitText.text = StatManager.DefenseUnit.ToString() + "/" + StatManager.DefenseUnitMax.ToString() + "DU";
    }
}
