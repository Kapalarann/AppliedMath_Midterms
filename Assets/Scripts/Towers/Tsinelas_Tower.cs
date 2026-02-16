using UnityEngine;

public class Tsinelas : ITower
{
    public override void StartAttack()
    {
        animator.SetTrigger("onDirect");
        attacking = true;
    }
}
