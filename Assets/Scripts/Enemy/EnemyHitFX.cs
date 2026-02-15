using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyHitFX : MonoBehaviour
{
    private SkinnedMeshRenderer rend;
    private MaterialPropertyBlock mpb;

    [SerializeField] private float flashFadeSpeed = 10f;

    private float flashValue;
    private float currentFlash;

    [Header("Target")]
    [SerializeField] private Transform model;

    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float strength = 0.25f;
    [SerializeField] private AnimationCurve falloff = AnimationCurve.EaseInOut(0, 1, 1, 0);

    float timer;
    float originalY;

    void Awake()
    {

        originalY = model.localPosition.y;

        rend = GetComponent<SkinnedMeshRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        currentFlash = Mathf.MoveTowards(currentFlash, 0f, flashFadeSpeed * Time.deltaTime);
        ApplyFlash();
        UpdateShake();
    }

    [Button("TriggerFlash")]
    public void TriggerFlash()
    {
        timer = Mathf.Max(timer, duration);
        currentFlash = 1f;
        ApplyFlash();
        UpdateShake();
    }

    void ApplyFlash()
    {
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            rend.GetPropertyBlock(mpb, i);
            mpb.SetFloat("_FlashValue", currentFlash);
            rend.SetPropertyBlock(mpb, i);
        }
    }

    void UpdateShake()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            float t = 1f - (timer / duration);
            float damper = falloff.Evaluate(t);

            float yOffset = Random.Range(-1f, 1f) * strength * damper;

            Vector3 pos = model.localPosition;
            pos.y = originalY + yOffset;
            model.localPosition = pos;
        }
        else
        {
            Vector3 pos = model.localPosition;
            pos.y = originalY;
            model.localPosition = pos;
        }
    }
}
