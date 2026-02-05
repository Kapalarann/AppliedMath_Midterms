using UnityEngine;

public abstract class ITower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float cooldown;
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] float range;

    private float timer;

    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    public Transform target;

    public virtual void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if(timer > cooldown)
        {
            timer -= cooldown;
            Shoot();
        }
    }

    public virtual void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Projectile projectile = proj.GetComponent<Projectile>();

        Vector3 ballisticDir = Vector3.zero;
        ballisticDir = Computations.PredictiveBallisticAim(
                shootPoint,
                target.GetComponent<Enemy>(),
                speed);

        projectile.velocity = ballisticDir * speed;
        projectile.damage = damage;
    }
}
