using Unity.VisualScripting;
using UnityEngine;

public static class Computations
{
    public static Vector3 CalculateBallisticAngle(Vector3 target, Vector3 origin, float speed)
    {
        Vector3 toTarget = target - origin;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float yHeight = toTarget.y;
        float xzDist = toTargetXZ.magnitude;

        float speedSq = speed * speed;
        float discriminant = speedSq * speedSq - gravity * (gravity * xzDist * xzDist + 2f * yHeight * speedSq);

        Vector3 dir;

        if (discriminant >= 0f)
        {
            float discRoot = Mathf.Sqrt(discriminant);
            float lowAngle = Mathf.Atan2(speedSq - discRoot, gravity * xzDist);

            dir = (toTargetXZ.normalized * Mathf.Cos(lowAngle)) + (Vector3.up * Mathf.Sin(lowAngle));
        }
        else
        {
            dir = Vector3.forward;
        }
        return dir.normalized;
    }

    public static Vector3 PredictEnemyPositionOnPath(
    Transform origin,
    Enemy enemy,
    float projectileSpeed)
    {
        Vector3 originPos = origin.position;

        // Initial guess: straight-line time
        Vector3 enemyPos = enemy.currentPath.GetPointOnPath(enemy.progress);
        float time = Vector3.Distance(originPos, enemyPos) / projectileSpeed;

        // Distance enemy travels along the path in that time
        float travelDistance = enemy.speed * time;

        // Convert distance → progress delta
        float progressDelta = travelDistance / enemy.currentPath.getLength();

        float predictedProgress = Mathf.Clamp01(enemy.progress + progressDelta);

        return enemy.currentPath.GetPointOnPath(predictedProgress);
    }

    public static Vector3 PredictiveBallisticAimOnPath(
    Transform origin,
    Enemy enemy,
    float projectileSpeed,
    int maxIterations = 15,
    float tolerance = 0.001f)
    {
        Vector3 originPos = origin.position;

        float predictedProgress = enemy.progress;
        Vector3 predictedPos = enemy.currentPath.GetPointOnPath(predictedProgress);
        predictedPos.y = enemy.targetPoint.position.y;

        for (int i = 0; i < maxIterations; i++)
        {
            float distance = Vector3.Distance(originPos, predictedPos);

            Vector3 toTarget = predictedPos - originPos;
            Vector3 dir = CalculateBallisticAngle(predictedPos, originPos, projectileSpeed);
            float horizontalDistance = new Vector3(toTarget.x, 0f, toTarget.z).magnitude;
            float horizontalSpeed = projectileSpeed * new Vector3(dir.x, 0f, dir.z).magnitude;
            float flightTime = horizontalDistance / horizontalSpeed;

            float newProgress = Mathf.Clamp01(enemy.progress + enemy.speed * flightTime);
            Vector3 newPredictedPos = enemy.currentPath.GetPointOnPath(newProgress);
            newPredictedPos.y = enemy.targetPoint.position.y;

            if ((newPredictedPos - predictedPos).sqrMagnitude < tolerance * tolerance)
            {
                predictedPos = newPredictedPos;
                predictedProgress = newProgress;
                Debug.Log("Tolerance met after " + (i+1) + " iterations.");
                break;
            }

            predictedPos = newPredictedPos;
            predictedProgress = newProgress;
        }

        // Final ballistic direction
        return CalculateBallisticAngle(predictedPos, originPos, projectileSpeed);
    }


    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        return
            uu * u * p0 +
            3f * uu * t * p1 +
            3f * u * tt * p2 +
            tt * t * p3;
    }

}