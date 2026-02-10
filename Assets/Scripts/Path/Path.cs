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
    private List<float> segmentLengths = new List<float>();
    private float totalLength;

    private const int initPointsCount = 4;

    private void Start()
    {
        RecalculateLengths();
    }

    public void Reset()
    {
        points.Clear();
        for(int i = 0; i < initPointsCount; ++i)
        {
            points.Add(new Vector3(i + 1, 0, 0));
        }
    }

    public int getPointsCount() { return points.Count; }
    public Vector3 getPoint(int i) { return points[i]; }
    public void setPoint(int i, Vector3 v) { points[i] = v; }
    public int getSegmentCount() { return points.Count / 3; }
    public float getLength() {  return totalLength; }

    public void addSegment()
    {
        Vector3 startingPoint = points[points.Count - 1];
        float offset = 5f;
        points.Add(new Vector3(startingPoint.x, startingPoint.y, startingPoint.z + offset));
        points.Add(new Vector3(startingPoint.x + offset, startingPoint.y, startingPoint.z + offset));
        points.Add(new Vector3(startingPoint.x + offset, startingPoint.y, startingPoint.z));

        RecalculateLengths();
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

        RecalculateLengths();
    }

    private float ApproximateBezierLength(
    Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int steps = 10)
    {
        float length = 0f;
        Vector3 prev = p0;

        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 point = Computations.CubicBezier(p0, p1, p2, p3, t);
            length += Vector3.Distance(prev, point);
            prev = point;
        }

        return length;
    }

    public void RecalculateLengths()
    {
        segmentLengths.Clear();
        totalLength = 0f;

        int segmentCount = getSegmentCount();

        for (int i = 0; i < segmentCount; ++i)
        {
            int idx = i * 3;
            float len = ApproximateBezierLength(
                points[idx],
                points[idx + 1],
                points[idx + 2],
                points[idx + 3]
            );

            segmentLengths.Add(len);
            totalLength += len;
        }
    }

    public Vector3 GetPointOnPath(float progress)
    {
        if (segmentLengths.Count == 0)
            return Vector3.zero;

        progress = Mathf.Clamp01(progress);
        float targetDistance = progress * totalLength;

        float accumulated = 0f;

        for (int i = 0; i < segmentLengths.Count; i++)
        {
            float segmentLength = segmentLengths[i];

            if (accumulated + segmentLength >= targetDistance)
            {
                float localDistance = targetDistance - accumulated;
                float localT = localDistance / segmentLength;

                int idx = i * 3;
                return transform.TransformPoint(Computations.CubicBezier(
                    points[idx],
                    points[idx + 1],
                    points[idx + 2],
                    points[idx + 3],
                    localT
                ));
            }

            accumulated += segmentLength;
        }

        // Fallback: first point
        return transform.TransformPoint(points[0]);
    }

    private void OnDrawGizmos()
    {
        if (points == null || points.Count < 4)
            return;

        Gizmos.color = Color.cyan;

        int segmentCount = getSegmentCount();
        const int resolution = 20; // higher = smoother curve

        for (int s = 0; s < segmentCount; s++)
        {
            int idx = s * 3;

            Vector3 p0 = points[idx];
            Vector3 p1 = points[idx + 1];
            Vector3 p2 = points[idx + 2];
            Vector3 p3 = points[idx + 3];

            Vector3 previousPoint = p0;

            for (int i = 1; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                Vector3 point = Computations.CubicBezier(p0, p1, p2, p3, t);

                Gizmos.DrawLine(transform.TransformPoint(previousPoint), transform.TransformPoint(point));
                previousPoint = point;
            }
        }
    }

}
