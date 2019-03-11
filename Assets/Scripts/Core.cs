using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : Entity
{
    public bool playerOwned = false;
    public Text victoryText;
    public const float CoreHealthDamageScoreMultiplier = 20000f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float amount)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        float startHealthFrac = unit.Health / unit.maxHealth;
        unit.Health -= amount;
        float endHealthFrac = unit.Health / unit.maxHealth;
        unit.healthBar.fillAmount = (unit.Health / unit.maxHealth);
        if (playerOwned)
        {
            StatManager.OpponentScore += Mathf.FloorToInt((startHealthFrac - endHealthFrac) * CoreHealthDamageScoreMultiplier);
        } else
        {
            StatManager.Score += Mathf.FloorToInt((startHealthFrac - endHealthFrac) * CoreHealthDamageScoreMultiplier);
        }
        if (unit.Health <= 0f)
        {
            Kill();
        }
        
    }

    public override void Kill()
    {
        //StatManager.Gold += unit.value;
        if (playerOwned)
        {
            StatManager.victoryState = StatManager.VictoryState.CPU;
            victoryText.text = "Game Over!\nScore: " + StatManager.Score;
        } else
        {
            StatManager.victoryState = StatManager.VictoryState.Player;
            victoryText.text = "Player Wins!";
        }
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        unit.InitHealth();
    }
}
