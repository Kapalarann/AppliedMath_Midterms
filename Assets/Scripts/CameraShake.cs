using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public Vector3 originalPos;

    void Awake()
    {
        instance = this;
    }

    public void Shake(float duration, float strength)
    {
        originalPos = transform.position;
        StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float time = 0f;

        while (time < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * strength;
            transform.localPosition = originalPos + randomOffset;

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
