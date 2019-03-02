using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Unit unit;

    // Start is called before the first frame update
    protected void Start()
    {
        unit.InitHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amount)
    {
        unit.Health -= amount;
        unit.healthBar.fillAmount = (unit.Health / unit.maxHealth);

        if (unit.Health <= 0f)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        //StatManager.Gold += unit.value;
        StatManager.DefenseUnit -= unit.defenseUnit;
        Destroy(gameObject);
    }
}
