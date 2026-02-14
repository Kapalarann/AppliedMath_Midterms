using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy instance;

    public int money;

    private void Awake()
    {
        instance = this;
    }
}
