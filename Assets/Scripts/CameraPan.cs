using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [Header("Pan Settings")]
    public Transform startPoint;       // Where the camera starts
    public Transform endPoint;         // Where the camera ends
    public float duration = 2f;        // Time to complete the pan
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Easing

    private float timer = 0f;
    private bool isPanning = false;

    void Update()
    {
        if (isPanning)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float curveT = curve.Evaluate(t);

            // Interpolate position
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, curveT);

            // Interpolate rotation
            transform.rotation = Quaternion.Slerp(startPoint.rotation, endPoint.rotation, curveT);

            // Stop panning when done
            if (t >= 1f)
            {
                isPanning = false;
            }
        }
    }

    // Call this to start the pan
    public void StartPan()
    {
        timer = 0f;
        isPanning = true;
    }
}