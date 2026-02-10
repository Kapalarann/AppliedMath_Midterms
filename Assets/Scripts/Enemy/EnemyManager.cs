using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        instance = this;
    }

    public void addEnemy(Enemy e) { enemies.Add(e); }
    public void removeEnemy(Enemy e) { enemies.Remove(e); }
}
