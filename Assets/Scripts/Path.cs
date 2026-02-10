using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> points = new List<Vector3>()
    {
        new Vector3(1f, 0f, 0f),
        new Vector3(2f, 0f, 0f),
        new Vector3(3f, 0f, 0f),
        new Vector3(4f, 0f, 0f)
    };

    private const int initPointsCount = 4;

    public void Reset()
    {
        points.Clear();
        for(int i = 0; i < initPointsCount; ++i)
        {
            points.Add(new Vector3(i + 1, 0, 0));
        }
    }

    public int getPointsCount()
    {
        return points.Count;
    }

    public Vector3 getPoint(int i)
    {
        return points[i];
    }

    public void setPoint(int i, Vector3 v)
    {
        points[i] = v;
    }

    public int getSegmentCount()
    {
        return points.Count / 3;
    }

    public void addSegment()
    {
        Vector3 startingPoint = points[points.Count - 1];
        float offset = 2f;
        points.Add(new Vector3(startingPoint.x, startingPoint.y, startingPoint.z + offset));
        points.Add(new Vector3(startingPoint.x + offset, startingPoint.y, startingPoint.z + offset));
        points.Add(new Vector3(startingPoint.x + offset, startingPoint.y, startingPoint.z));
    }

    public void removeSegment()
    {
        int pointCount = points.Count;
        if(getSegmentCount() > 1)
        {
            points.RemoveAt(pointCount - 1);
            points.RemoveAt(pointCount - 2);
            points.RemoveAt(pointCount - 3);
        }
    }
}
