using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Entity
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Kill()
    {
        //StatManager.Gold += unit.value;
        Debug.Log("Game Over!");
        Destroy(gameObject);
    }
}
