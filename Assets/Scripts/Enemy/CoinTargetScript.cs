using UnityEngine;

public class CoinTargetScript : MonoBehaviour
{
    public static CoinTargetScript Instance;

    public RectTransform currencyUI;
    public Camera uiCamera;
    public Camera worldCamera;

    public Transform target;

    void Awake()
    {
        Instance = this;
        target = transform;
    }

    void Update()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, currencyUI.position);

        Vector3 worldPos = worldCamera.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, 10f)
        );

        transform.position = worldPos;
    }
}
