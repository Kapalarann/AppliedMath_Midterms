using System;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] TowerPacement[] towerPrefabs;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower(0);
        }
        if (Input.GetMouseButtonDown(2))
        {
            PlaceTower(1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            PlaceTower(2);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LevelUp();
        }
    }

    private void PlaceTower(int index)
    {
        TowerPacement t = towerPrefabs[index];

        foreach (Space s in SpaceManager.instance.spaces) {
            if (s == null) continue;
            if (s.filled)
            {
                Debug.Log("Space Already filled.");
                continue;
            }

            if (t.cost > Economy.instance.money)
            {
                Debug.Log("No money.");
                return;
            }

            if (Vector3.Distance(
                Computations.MouseToGround(Camera.main, s.transform.position.y),
                s.transform.position)
                < s.radius)
            {
                GameObject tower = Instantiate(t.prefab, s.transform);
                s.Fill();

                Economy.instance.SpendMoney(t.cost);
                Debug.Log(t.prefab.name + " placed | Money Remaining: " + Economy.instance.money);
            }
        }
    }

    private void LevelUp()
    {
        foreach (Space s in SpaceManager.instance.spaces)
        {
            if (!s.filled) continue;

            if (Vector3.Distance(
                Computations.MouseToGround(Camera.main, s.transform.position.y),
                s.transform.position)
                < s.radius)
            {
                ITower tower = s.GetComponentInChildren<ITower>();

                int nextLevel = tower.level + 1;

                if (nextLevel > tower.towerDatum.Length)
                {
                    Debug.Log("Tower is max level.");
                    return;
                }

                if (tower.towerDatum[nextLevel - 1].cost > Economy.instance.money)
                {
                    Debug.Log("No money.");
                    return;
                }

                Economy.instance.SpendMoney(tower.towerDatum[nextLevel - 1].cost);
                tower.UpgradeTower();
                
                Debug.Log(tower.towerDatum + " upgraded to level" + tower.level 
                    + " | Money Remaining: " + Economy.instance.money);

                return;
            }
        }
    }
}

[Serializable]
class TowerPacement
{
    public GameObject prefab;
    public int cost;
}