using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public bool usesGravity;
    public bool rotatesTowards;
    public Vector3 rotationSpeed;

    [Header("Stats")]
    public string projectileTag;
    public float hitRadius;
    public float damage;
    public int pierce;
    public Vector3 velocity;
    public float duration;
    public float timer;
    public bool isMoving = true;
    public int currentPierce;

    [Header("VFX")]
    [SerializeField] public spawnVFX onHitVFX;
    [SerializeField] public float VFXOffSet;

    [Header("References")]
    public Transform model;
    public List<Enemy> alreadyHit = new List<Enemy>();

    public virtual void SetValues()
    {
        timer = duration;
        currentPierce = pierce;
        alreadyHit.Clear();

        if (rotatesTowards) model.rotation *= Computations.rotateYawTowardsVelocity(velocity);
    }

    public virtual void FixedUpdate()
    {
        if (timer <= 0f) Des();

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
        foreach (Enemy enemy in EnemyManager.instance.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.targetPoint.position) <= hitRadius
                && !alreadyHit.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                alreadyHit.Add(enemy);

                PlayHitVFX();

                currentPierce--;
                if (currentPierce <= 0)
                {
                    Des();
                    break;
                }
            }
        }
    }

    public virtual void PlayHitVFX()
    {
        if (onHitVFX == null || onHitVFX.prefab == null) return;
        GameObject vfx = Instantiate(onHitVFX.prefab);

        Vector3 pos = transform.position;
        pos.y = VFXOffSet;
        vfx.transform.position = pos;

        Vector3 scale = vfx.transform.localScale;
        scale *= onHitVFX.scaleMult;
        vfx.transform.localScale = scale;
    }

    public virtual void Des()
    {
        ObjectPool.Instance.Enqueue(projectileTag, gameObject);
    }
}

[Serializable]
public class spawnVFX
{
    public GameObject prefab;
    public float scaleMult;
}