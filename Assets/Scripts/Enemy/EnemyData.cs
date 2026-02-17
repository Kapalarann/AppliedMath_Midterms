using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;    // The enemy prefab to spawn
    public int maxHP = 10;       // Enemy health
    [Range(0f, 0.1f)] public float speed = 0.03f;     // Movement speed
    public int goldReward = 10;   // Gold given when killed
    public float spawnCost = 1f;    // Cost to spawn this enemy
    public float delayMult = 1f;
}
