using UnityEngine;

public class Space : MonoBehaviour
{
    public float radius;
    public bool filled = false;
    [SerializeField] GameObject placeHolder;

    public void Fill()
    {
        filled = true;
        transform.rotation = Quaternion.identity;
        placeHolder.SetActive(false);
    }
}
