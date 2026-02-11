using Unity.Collections;
using UnityEngine;

public abstract class ITower : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool prefersHighAngle = false;

    [Header("Stats")]
    [SerializeField] float cooldown;
    [SerializeField] float hitRadius;
    [SerializeField] float damage;
    [SerializeField] int pierce;
    [SerializeField] float duration;
    [SerializeField] private float _range;
    public float Range{
        get => _range;
        set
        {
            _range = value;
            Speed = Mathf.Sqrt(_range * gravity); // v = sqrt(R * g)
        }}
    [SerializeField] private float speed;
    public float Speed{
        get => speed;
        private set => speed = value; // optional: read-only from outside
    }
    public float speedMult = 1f;
    private float gravity = 9.81f;

    private float timer;

    [Header("References")]
    [SerializeField] public Transform shootPoint;
    [SerializeField] public GameObject projectilePrefab;
    public Transform target;

    private void OnValidate()
    {
        Speed = Mathf.Sqrt(_range * gravity);
    }

    public virtual void FixedUpdate()
    {
        if(timer < cooldown) timer += Time.fixedDeltaTime;
        if(timer >= cooldown)
        {
            AcquireTarget();
            if (target == null) return;
            Shoot();
            timer -= cooldown;
        }
    }

    private void AcquireTarget()
    {
        Enemy first = null;
        float firstProg = 0f;

        foreach (Enemy e in EnemyManager.instance.enemies)
        {
            if (Vector3.Distance(e.targetPoint.position, shootPoint.position) <= Range && // within range
                e.progress > firstProg) // & farthest progress
            {
                first = e;
                firstProg = e.progress;
            }
        }

        if(first != null) target = first.transform;
    }

    public virtual void Shoot()
    {
        Vector3 a = target.GetComponent<Enemy>().targetPoint.position;
        Vector3 b = shootPoint.position;
        a.y = 0f; b.y = 0f;
        float dist = Vector3.Distance(a, b);
        if (dist > _range) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Projectile projectile = proj.GetComponent<Projectile>();

        projectile.velocity = ShootDir() * Speed * speedMult;
        projectile.hitRadius = hitRadius;
        projectile.damage = damage;
        projectile.pierce = pierce;
        projectile.duration = duration;

        projectile.Instantiate();
    }

    public virtual Vector3 ShootDir()
    {
        return Computations.PredictiveBallisticAimOnPath(
                shootPoint,
                target.GetComponent<Enemy>(),
                Speed,
                prefersHighAngle);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(shootPoint.position, _range);
    }
}
