using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy instance;

    public float money;
    public bool unlimitedMoney = false;

    private void Awake()
    {
        instance = this;
    }

    public void SpendMoney(float cost)
    {
        if (unlimitedMoney) return;
        money -= cost;
    }
}
