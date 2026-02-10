using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    private float currentBudget;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        foreach (Wave wave in waves)
        {
            yield return new WaitForSeconds(wave.startDelay);
            yield return StartCoroutine(SpawnWave(wave));
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        currentBudget = wave.waveData.spawnBudget;

        while (currentBudget > 0)
        {
            EnemyData enemy = PickAffordableEnemy(wave.waveData);
            if (enemy == null)
                yield break;

            Path path = PickPath(wave.paths);
            SpawnEnemy(enemy, path);

            currentBudget -= enemy.spawnCost;
            yield return new WaitForSeconds(Mathf.Lerp
                (wave.waveData.spawnDelay.x,
                wave.waveData.spawnDelay.y,
                Random.Range(0f,1f))
                * enemy.spawnCost
                );
        }
    }

    private EnemyData PickAffordableEnemy(WaveData waveData)
    {
        List<EnemyData> affordable = new List<EnemyData>();

        foreach (var enemy in waveData.possibleEnemies)
        {
            if (enemy.spawnCost <= currentBudget)
                affordable.Add(enemy);
        }

        if (affordable.Count == 0)
            return null;

        return affordable[Random.Range(0, affordable.Count)];
    }

    private Path PickPath(Path[] paths)
    {
        if (paths == null || paths.Length == 0)
        {
            Debug.LogWarning("Wave has no paths assigned!");
            return null;
        }

        return paths[Random.Range(0, paths.Length)];
    }

    private void SpawnEnemy(EnemyData enemyData, Path path)
    {
        GameObject enemyGO = Instantiate(
            enemyData.prefab,
            path.transform.position,
            Quaternion.identity
        );

        Enemy enemy = enemyGO.GetComponent<Enemy>();
        if (enemy == null) return;
        
        enemy.speed = enemyData.speed;
        enemy.maxHP = enemyData.maxHP;
        enemy.goldReward = enemyData.goldReward;
        enemy.Initialize(path);
    }
}

[System.Serializable]
public class Wave
{
    public WaveData waveData;

    [Tooltip("Delay before this wave starts")]
    public float startDelay = 10f;

    [Tooltip("Paths available for this wave")]
    public Path[] paths;
}