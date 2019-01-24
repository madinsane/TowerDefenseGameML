using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity
{
    public Structure wall;
    // Start is called before the first frame update
    new void Start()
    {
        wall.InitHealth();
        Waypoint.points.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new void Kill()
    {
        Waypoint.points.Remove(transform);
        //StatManager.Gold += wall.value;
        Destroy(gameObject);
    }
}
