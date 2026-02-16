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

    public override void StartAttack()
    {
        animator.SetTrigger("onOverhead");
        attacking = true;
    }
}
