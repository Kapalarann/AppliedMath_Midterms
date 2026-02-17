using Sirenix.OdinInspector;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float arcHeight = 2f;
    [ReadOnly]
    private float Amount;

    private Transform target;
    private Vector3 startPos;
    private float travelTime;
    private float elapsed;

    public void SetTarget(Transform uiTarget)
    {
        target = uiTarget;
        startPos = transform.position;
    }

    void Update()
    {
        if (target == null) return;

        elapsed += Time.deltaTime;
        travelTime += Time.deltaTime * moveSpeed;

        Vector3 targetPos = target.position;

        Vector3 pos = Vector3.Lerp(startPos, targetPos, travelTime);

        pos.y += Mathf.Sin(travelTime * Mathf.PI) * arcHeight;

        transform.position = pos;

        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
        {
            ReachTarget();
        }
    }

    public void setAmount(float amount)
    {
        Amount = amount;
    }

    void ReachTarget()
    {

        FindFirstObjectByType<GameUIScript>().AddCurrency(Amount);
        Destroy(gameObject);
    }
}