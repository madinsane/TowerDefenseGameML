using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopText : MonoBehaviour
{
    public Text text;
    public void ChangeText(string newText)
    {
        newText = newText.Replace("\\n", "\n");
        text.text = newText;
    }
}
