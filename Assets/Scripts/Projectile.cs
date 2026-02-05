using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float damage;
    public Vector3 velocity;

    public virtual void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        velocity += Physics.gravity * Time.fixedDeltaTime;

        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    //if enemy within collisionRange, destroy & deal damage
}
