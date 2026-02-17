using UnityEngine;

public class Bilao_Tower : ITower
{
    public Vector3 targetPos;

    [SerializeField] int maxAttempt = 15;
    private float targetProgress;
    private Path targetPath;

    public override void FixedUpdate()
    {
        if (timer < cooldown) timer += Time.fixedDeltaTime;
        if (timer >= cooldown)
        {
            AcquireTarget();
            if (target == null) return;
            if (attacking) return;
            StartAttack();
            timer -= cooldown;
        }
    }

    public override void StartAttack()
    {
        animator.SetTrigger("onFrisbee");
        attacking = true;
    }

    public override void AcquireTarget()
    {
        for (int i = 0; i < maxAttempt; ++i)
        {
            float testProgress = Random.Range(0.0f, 1.0f);

            foreach (Path p in PathManager.instance.paths)
            {
                if (Vector3.Distance(shootPoint.position, p.GetPointOnPath(testProgress)) <= Range)
                {
                    targetPos = p.GetPointOnPath(testProgress);
                    targetProgress = testProgress;
                    targetPath = p;
                    return;
                }
            }
        }
    }
    public override void Shoot(AnimationEvent animationEvent)
    {
        Vector3 dir = ShootDir();
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir);

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Bilao_Projectile projectile = proj.GetComponent<Bilao_Projectile>();

        projectile.velocity = ShootDir() * Speed * speedMult;
        projectile.hitRadius = hitRadius;
        projectile.damage = damage;
        projectile.pierce = pierce;
        projectile.duration = duration;
        projectile.progressTarget = targetProgress;
        projectile.path = targetPath;

        projectile.SetValues();
    }

    public override Vector3 ShootDir()
    {
        return Computations.CalculateBallisticAngle(
                targetPos,
                shootPoint.position,
                Speed,
                prefersHighAngle);
    }
}
