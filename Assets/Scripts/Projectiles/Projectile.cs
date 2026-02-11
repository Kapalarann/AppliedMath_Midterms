using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float hitRadius;
    public float damage;
    public int pierce;
    public Vector3 velocity;
    public bool usesGravity;
    public Vector3 rotationSpeed;
    public float duration;
    public float timer;

    [Header("References")]
    public Transform model;
    public List<Enemy> alreadyHit = new List<Enemy>();

    public virtual void Instantiate()
    {
        timer = duration;
        Destroy(gameObject, duration);

        model.rotation *= Computations.rotateYawTowardsVelocity(velocity);
    }

    public virtual void FixedUpdate()
    {
        if(usesGravity) velocity += Physics.gravity * Time.fixedDeltaTime;
        timer -= Time.fixedDeltaTime;

        transform.Translate(velocity * Time.fixedDeltaTime);
        model.Rotate(rotationSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        CheckEnemy();
    }

    public virtual void CheckEnemy()
    {
        foreach (var enemy in EnemyManager.instance.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.targetPoint.position) <= hitRadius
                && !alreadyHit.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                alreadyHit.Add(enemy);

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
