using UnityEngine;

public class Bilao_Projectile : Projectile
{
    public Path path;
    public float progressTarget;

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, path.GetPointOnPath(progressTarget)) <= hitRadius)
        {
            isMoving = false;
        }
    }

    public override void CheckEnemy()
    {
        foreach (var enemy in EnemyManager.instance.enemies)
        {
            Vector3 a = transform.position;
            Vector3 b = enemy.targetPoint.position;
            a.y = 0f; b.y = 0f;

            if (Vector3.Distance(a, b) <= hitRadius)
            {
                enemy.TakeDamage(damage);

                pierce--;
                if (pierce <= 0)
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
