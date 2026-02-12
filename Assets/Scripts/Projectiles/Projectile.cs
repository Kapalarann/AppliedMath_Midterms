using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public bool usesGravity;
    public bool rotatesTowards;
    public Vector3 rotationSpeed;

    [Header("Stats")]
    public float hitRadius;
    public float damage;
    public int pierce;
    public Vector3 velocity;
    public float duration;
    public float timer;
    public bool isMoving = true;

    [Header("References")]
    public Transform model;
    public List<Enemy> alreadyHit = new List<Enemy>();

    public virtual void SetValues()
    {
        timer = duration;
        Destroy(gameObject, duration);

        if (rotatesTowards) model.rotation *= Computations.rotateYawTowardsVelocity(velocity);
    }

    public virtual void FixedUpdate()
    {
        if(!isMoving) return;
        if(usesGravity) velocity += Physics.gravity * Time.fixedDeltaTime;
        timer -= Time.fixedDeltaTime;

        transform.Translate(velocity * Time.fixedDeltaTime);
        model.Rotate(rotationSpeed * Time.fixedDeltaTime);
    }

    public virtual void Update()
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
