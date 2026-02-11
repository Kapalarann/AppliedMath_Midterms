using Unity.VisualScripting;
using UnityEngine;

public static class Computations
{
    public static Vector3 CalculateBallisticAngle(
    Vector3 target,
    Vector3 origin,
    float speed,
    bool prefersHighAngle = false)
    {
        Vector3 toTarget = target - origin;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float yHeight = toTarget.y;
        float xzDist = toTargetXZ.magnitude;

        float speedSq = speed * speed;
        float discriminant =
            speedSq * speedSq -
            gravity * (gravity * xzDist * xzDist + 2f * yHeight * speedSq);

        if (discriminant < 0f || xzDist < 0.001f)
            return Vector3.forward;

        float discRoot = Mathf.Sqrt(discriminant);

        float angle = prefersHighAngle
            ? Mathf.Atan2(speedSq + discRoot, gravity * xzDist)
            : Mathf.Atan2(speedSq - discRoot, gravity * xzDist);

        Vector3 dir =
            toTargetXZ.normalized * Mathf.Cos(angle) +
            Vector3.up * Mathf.Sin(angle);

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
    bool prefersHighAngle = false,
    int maxIterations = 15,
    float tolerance = 0.001f)
    {
        Vector3 originPos = origin.position;

        float predictedProgress = enemy.progress;
        Vector3 predictedPos =
            enemy.currentPath.GetPointOnPath(predictedProgress);
        predictedPos.y = enemy.targetPoint.position.y;

        for (int i = 0; i < maxIterations; i++)
        {
            Vector3 toTarget = predictedPos - originPos;

            Vector3 dir = CalculateBallisticAngle(
                predictedPos,
                originPos,
                projectileSpeed,
                prefersHighAngle);

            float horizontalDistance =
                new Vector3(toTarget.x, 0f, toTarget.z).magnitude;

            float horizontalSpeed =
                projectileSpeed *
                new Vector3(dir.x, 0f, dir.z).magnitude;

            if (horizontalSpeed < 0.001f)
                break;

            float flightTime = horizontalDistance / horizontalSpeed;

            float newProgress =
                Mathf.Clamp01(enemy.progress + enemy.speed * flightTime);

            Vector3 newPredictedPos =
                enemy.currentPath.GetPointOnPath(newProgress);
            newPredictedPos.y = enemy.targetPoint.position.y;

            if ((newPredictedPos - predictedPos).sqrMagnitude <
                tolerance * tolerance)
            {
                predictedPos = newPredictedPos;
                predictedProgress = newProgress;
                break;
            }

            predictedPos = newPredictedPos;
            predictedProgress = newProgress;
        }

        return CalculateBallisticAngle(
            predictedPos,
            originPos,
            projectileSpeed,
            prefersHighAngle);
    }

    public static Vector3 PredictiveStraightAimOnPath(
    Transform origin,
    Enemy enemy,
    float projectileSpeed,
    int maxIterations = 15,
    float tolerance = 0.001f)
    {
        Vector3 originPos = origin.position;

        float predictedProgress = enemy.progress;
        Vector3 predictedPos =
            enemy.currentPath.GetPointOnPath(predictedProgress);

        // Lock everything to XZ plane
        predictedPos.y = originPos.y;

        for (int i = 0; i < maxIterations; i++)
        {
            Vector3 toTarget = predictedPos - originPos;
            toTarget.y = 0f;

            float distance = toTarget.magnitude;
            if (distance < 0.001f)
                break;

            float flightTime = distance / projectileSpeed;

            float newProgress =
                Mathf.Clamp01(enemy.progress + enemy.speed * flightTime);

            Vector3 newPredictedPos =
                enemy.currentPath.GetPointOnPath(newProgress);
            newPredictedPos.y = originPos.y;

            if ((newPredictedPos - predictedPos).sqrMagnitude <
                tolerance * tolerance)
            {
                predictedPos = newPredictedPos;
                predictedProgress = newProgress;
                break;
            }

            predictedPos = newPredictedPos;
            predictedProgress = newProgress;
        }

        Vector3 finalDir = predictedPos - originPos;
        finalDir.y = 0f;

        return finalDir.normalized;
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

    public static Quaternion rotateYawTowardsVelocity(Vector3 v)
    {
        Vector3 flatVel = v;
        flatVel.y = 0f;

        if (flatVel.sqrMagnitude > 0.0001f)
        {
            float yaw = Mathf.Atan2(flatVel.x, flatVel.z) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, yaw, 0f);
        }

        return Quaternion.Euler(0f, 0f, 0f);
    }
}