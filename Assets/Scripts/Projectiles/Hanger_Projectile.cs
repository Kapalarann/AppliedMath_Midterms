using UnityEngine;

public class Hanger_Projectile : Projectile
{
    private bool flipped = false;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (flipped) return;
        if (timer < (duration / 2) )
        {
            velocity *= -1f;
            alreadyHit.Clear();
            flipped = true;
        }
    }
}
