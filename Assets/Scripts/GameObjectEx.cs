using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectEx
{
    public static LineRenderer DrawCircle(this GameObject container, float radius, float lineWidth, Material material, LineRenderer _line = null)
    {
        int segments = 360;
        LineRenderer line;
        if (_line == null)
        {
            line = container.AddComponent<LineRenderer>();
        } else
        {
            line = _line;
        }
        line.transform.eulerAngles = new Vector3(-90f, 0, 0);
        line.sortingLayerName = "Indicators";
        line.material = material;
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);

        return line;
    }
}
