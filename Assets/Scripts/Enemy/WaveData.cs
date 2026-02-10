using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Enemies/Wave Data")]
public class WaveData : ScriptableObject
{
    public Vector2 spawnDelay = new Vector2(0.8f,1.2f);           // Delay between spawns
    public float spawnBudget = 10f;            // Maximum total cost for spawning enemies in this wave
    public EnemyData[] possibleEnemies;     // Which enemies can spawn in this wave
}