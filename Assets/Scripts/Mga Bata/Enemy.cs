using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public Path currentPath;
    [Range(0f, 1f)] public float progress = 0f;
    public float speed = 0.02f;

    [Header("References")]
    [SerializeField] public Transform targetPoint;

    public void Initialize(Path path)
    {
        EnemyManager.instance.addEnemy(this);
        currentPath = path;
    }

    private void FixedUpdate()
    {
        if (currentPath == null) return;

        progress = Mathf.Clamp01(progress + (speed * Time.fixedDeltaTime));
        if(progress == 1f) ReachEnd();
        transform.position = currentPath.GetPointOnPath(progress);
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
