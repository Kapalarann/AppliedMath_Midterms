using Sirenix.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float maxHP;
    [SerializeField] public int goldReward;
    private float HP;

    [Header("Movement")]
    public Path currentPath;
    [Range(0f, 1f)] public float progress = 0f;
    public float speed = 0.02f;
    private Vector3 previousPos;

    [Header("References")]
    [SerializeField] public Transform targetPoint;

    public void Initialize(Path path)
    {
        EnemyManager.instance.addEnemy(this);
        currentPath = path;
        previousPos = transform.position;

        HP = maxHP;
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
        HP -= damage;
        Debug.Log(name + " took " + damage + " damage. Currently at: " + HP + "/" + maxHP);
        if (HP < 0f) Destroy(gameObject);
    }

    private void ReachEnd()
    {
        Destroy(gameObject); 
        //player take damage
    }

    private void OnDestroy()
    {
        EnemyManager.instance.removeEnemy(this);
    }
}
