using UnityEngine;

public class Hanger : ITower
{
    public override Vector3 ShootDir()
    {
        return Computations.PredictiveStraightAimOnPath(
                shootPoint,
                target.GetComponent<Enemy>(),
                Speed);
    }
}
