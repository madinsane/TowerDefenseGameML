using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static List<Transform> points;
    public static List<Transform> opponentPoints;

    [Header("Setup")]
    public GameObject cores;
    public GameObject opponentCores;

    private void Awake()
    {
        points = new List<Transform>(cores.transform.childCount);
        for (int i = 0; i < cores.transform.childCount; i++)
        {
            points.Add(cores.transform.GetChild(i));
        }
        opponentPoints = new List<Transform>(opponentCores.transform.childCount);
        for (int i = 0; i < opponentCores.transform.childCount; i++)
        {
            opponentPoints.Add(opponentCores.transform.GetChild(i));
        }
    }

    public void AddWaypoint(Transform point)
    {
        points.Add(point);
        opponentPoints.Add(point);
    }

    public void RemoveWaypoint(Transform point)
    {
        if (points.Contains(point))
        {
            points.Remove(point);
        }
        if (opponentPoints.Contains(point))
        {
            opponentPoints.Remove(point);
        }
    }
}
