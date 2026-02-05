using UnityEngine;

public static class Computations
{
    public static Vector3 CalculateBallisticAngle(Vector3 target, Vector3 origin, float speed)
    {
        Vector3 toTarget = target - origin;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float yHeight = toTarget.y + 0.25f;
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

    public static Vector3 PredictiveBallisticAim(
    Transform origin,
    Enemy target,
    float projectileSpeed)
    {
        Vector3 originPos = origin.position;
        Vector3 targetPos = target.targetPoint.position;
        Vector3 targetVel = target.velocity;

        float time = Vector3.Distance(originPos, targetPos) / projectileSpeed;

        Vector3 predictedPos = targetPos + targetVel * time;

        Vector3 dir = CalculateBallisticAngle(predictedPos, originPos, projectileSpeed);

        Vector3 toPredicted = predictedPos - originPos;
        float horizontalSpeed = projectileSpeed * new Vector3(dir.x, 0f, dir.z).magnitude;
        float horizontalDistance = new Vector3(toPredicted.x, 0f, toPredicted.z).magnitude;

        if (horizontalSpeed > 0.001f)
        {
            time = horizontalDistance / horizontalSpeed;
            predictedPos = targetPos + targetVel * time;
            dir = CalculateBallisticAngle(predictedPos, originPos, projectileSpeed);
        }

        return dir;
    }


    public static Vector3 CalculateBallisticVelocity(Vector3 target, Vector3 origin, float timeToTarget, float gravityScale = 1f)
    {
        Vector3 displacement = target - origin;
        Vector3 displacementXZ = new Vector3(displacement.x, 0f, displacement.z);

        float gravity = Mathf.Abs(Physics.gravity.y * gravityScale);

        float vy = displacement.y / timeToTarget + 0.5f * gravity * timeToTarget;
        Vector3 vxz = displacementXZ / timeToTarget;

        return vxz + (Vector3.up * vy);
    }

}