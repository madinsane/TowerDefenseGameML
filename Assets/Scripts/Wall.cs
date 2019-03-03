using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Structure
{
    public StructureShopData wall;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        InvokeRepeating("Taunt", 0f, 0.5f);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    void Taunt()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= unit.range)
            {
                enemy.GetComponent<Enemy>().TauntTarget = gameObject.transform;                
            }
        }
    }

    new void Kill()
    {
        //StatManager.Gold += wall.value;
        Destroy(gameObject);
    }
}
