using Sirenix.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float maxHP;
    [SerializeField] public int goldReward;
    [SerializeField] public int damage;
    public float currentHP;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] int coinAmount = 5;

    [Header("Movement")]
    public Path currentPath;
    [Range(0f, 1f)] public float progress = 0f;
    public float speed = 0.02f;
    private Vector3 previousPos;

    [Header("References")]
    [SerializeField] public Transform targetPoint;
    private EnemyHitFX hitFX;

    private void Awake()
    {
        hitFX = GetComponentInChildren<EnemyHitFX>();
    }

    public void Initialize(Path path)
    {
        EnemyManager.instance.addEnemy(this);
        currentPath = path;
        previousPos = transform.position;

        currentHP = maxHP;
    }

    private void FixedUpdate()
    {
        if (currentPath == null) return;

        progress = Mathf.Clamp01(progress + (speed * Time.fixedDeltaTime));
        if(progress == 1f) ReachEnd();
        transform.position = currentPath.GetPointOnPath(progress);
        RotateFacing();
    }

    private void RotateFacing()
    {
        Vector3 dir = transform.position - previousPos;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f) { transform.rotation = Quaternion.LookRotation(dir); }
        previousPos = transform.position;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log(name + " took " + damage + " damage. Currently at: " + currentHP + "/" + maxHP);
        if (currentHP <= 0f) Destroy(gameObject);
        if (hitFX == null) return;
        hitFX.TriggerFlash();
    }

    private void ReachEnd()
    {
        Destroy(gameObject);
        GameUIScript.instance.SetHP(GameUIScript.instance.playerHP - damage);
    }

    private void OnDestroy()
    {
        EnemyManager.instance.removeEnemy(this);
        DropCoins();
    }


    void DropCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = Instantiate(
                coinPrefab,
                transform.position + Random.insideUnitSphere,
                Quaternion.identity
            );
            coin.GetComponent<Coin>().setAmount(goldReward);
            coin.GetComponent<Coin>().SetTarget(CoinTargetScript.Instance.target);
        }
    }
}
