using System;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] TowerPacement[] towerPrefabs;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower(towerPrefabs[0]);
        }
        if (Input.GetMouseButtonDown(2))
        {
            PlaceTower(towerPrefabs[1]);
        }
        if (Input.GetMouseButtonDown(1))
        {
            PlaceTower(towerPrefabs[2]);
        }
    }

    private void PlaceTower(TowerPacement t)
    {
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
                Computations.MouseToGround(Camera.main),
                s.transform.position)
                < s.radius)
            {
                GameObject tower = Instantiate(t.prefab, s.transform);
                Vector3 spawnPos = s.transform.position;
                spawnPos.y = 1f;
                tower.transform.position = spawnPos;

                Economy.instance.money -= t.cost;
                Debug.Log(t.prefab.name + " placed | Money Remaining: " + Economy.instance.money);
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