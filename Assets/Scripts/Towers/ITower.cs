using UnityEngine;

public abstract class ITower : MonoBehaviour
{
    [Header("Tower Data")]
    [SerializeField] public TowerData[] towerDatum;
    public int level = 0;

    [Header("Ballistics")]
    [SerializeField] public bool prefersHighAngle = false;

    [Header("Stats")]
    [SerializeField] public float cooldown;
    [SerializeField] public float hitRadius;
    [SerializeField] public float damage;
    [SerializeField] public int pierce;
    [SerializeField] public float duration;
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

    public float timer;

    [Header("References")]
    [SerializeField] public Transform shootPoint;
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimationReciever reciever;
    public bool attacking = false;
    public Transform target;

    private void Awake()
    {
        UpgradeTower();

        reciever.AttackFrame += Shoot;
        reciever.AttackEnd += AnimEnd;
    }

    private void OnDestroy()
    {
        reciever.AttackFrame -= Shoot;
        reciever.AttackEnd -= AnimEnd;
    }

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
            if (attacking) return;
            StartAttack();
            timer -= cooldown;
        }
    }

    public virtual void StartAttack()
    {
        animator.SetTrigger("onAttack");
        attacking = true;
    }

    public virtual void AcquireTarget()
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

    public virtual void Shoot(AnimationEvent animationEvent)
    {
        Vector3 a = target.GetComponent<Enemy>().targetPoint.position;
        Vector3 b = shootPoint.position;
        float dist = Vector3.Distance(a, b);
        if (dist > _range) return;

        Vector3 dir = ShootDir();
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir);

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();

        projectile.velocity = ShootDir() * Speed * speedMult;
        projectile.hitRadius = hitRadius;
        projectile.damage = damage;
        projectile.pierce = pierce;
        projectile.duration = duration;

        projectile.SetValues();
    }

    public virtual void AnimEnd(AnimationEvent animationEvent)
    {
        attacking = false;
    }

    public virtual Vector3 ShootDir()
    {
        return Computations.PredictiveBallisticAimOnPath(
                shootPoint,
                target.GetComponent<Enemy>(),
                Speed,
                prefersHighAngle);
    }

    public virtual void UpgradeTower()
    {
        if (level >= towerDatum.Length) return;
        level++;

        prefersHighAngle = towerDatum[level - 1].prefersHighAngle;

        cooldown = towerDatum[level - 1].cooldown;
        hitRadius = towerDatum[level - 1].hitRadius;
        damage = towerDatum[level - 1].damage;
        pierce = towerDatum[level - 1].pierce;
        duration = towerDatum[level - 1].duration;

        Range = towerDatum[level-1].range;
        speedMult = towerDatum[level - 1].speedMult;

        projectilePrefab = towerDatum[level - 1].projectilePrefab;

        if (animator != null)
            animator.speed = towerDatum[level - 1].animationSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(shootPoint.position, _range);
    }
}
