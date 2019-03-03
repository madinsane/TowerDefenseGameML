using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : AttackEntity
{
    public bool IsSelected { get; set; }
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Kill()
    {
        StatManager.Gold += unit.baseValue;
        StatManager.DefenseUnit -= unit.defenseUnit;
        Destroy(gameObject);
    }
}
