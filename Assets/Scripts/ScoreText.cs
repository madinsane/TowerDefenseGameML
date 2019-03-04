using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text text;

    public void Update()
    {
        text.text = "Score: " + StatManager.Score.ToString();
    }
}
