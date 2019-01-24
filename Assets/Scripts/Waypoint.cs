using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static List<Transform> points;

    private void Awake()
    {
        points = new List<Transform>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i));
        }
    }

    public void AddWaypoint(Transform point)
    {
        points.Add(point);
    }

    public void RemoveWaypoint(Transform point)
    {
        if (points.Contains(point))
        {
            points.Remove(point);
        }
    }
}
