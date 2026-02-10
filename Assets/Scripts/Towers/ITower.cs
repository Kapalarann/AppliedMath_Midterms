using Unity.Collections;
using UnityEngine;

public abstract class ITower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float cooldown;
    [SerializeField] float damage;
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
    private float gravity = 9.81f;

    private float timer;

    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
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
            timer -= cooldown;
            Shoot();
        }
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

        Vector3 ballisticDir = Vector3.zero;
        ballisticDir = Computations.PredictiveBallisticAimOnPath(
                shootPoint,
                target.GetComponent<Enemy>(),
                speed);

        projectile.velocity = ballisticDir * speed;
        projectile.damage = damage;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(shootPoint.position, _range);
    }
}
