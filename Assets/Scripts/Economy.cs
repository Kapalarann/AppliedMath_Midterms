using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy instance;

    public int money;
    public bool unlimitedMoney = false;

    private void Awake()
    {
        instance = this;
    }

    public void SpendMoney(int cost)
    {
        if (unlimitedMoney) return;
        money -= cost;
    }
}
