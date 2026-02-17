using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    private int currentWave = 1;
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
        List<Coroutine> runningPaths = new List<Coroutine>();

        foreach (var pathWave in wave.pathWaves)
        {
            Coroutine c = StartCoroutine(SpawnPath(pathWave));
            runningPaths.Add(c);
        }
        
        // Wait for all paths to finish spawning
        foreach (var c in runningPaths)
            yield return c;
    }

    private IEnumerator SpawnPath(PathWave pathWave)
    {
        float budget = pathWave.waveData.spawnBudget;

        while (budget > 0)
        {
            EnemyData enemy = PickAffordableEnemy(pathWave.waveData, budget);
            if (enemy == null)
                yield break;

            SpawnEnemy(enemy, pathWave.path);
            budget -= enemy.spawnCost;

            yield return new WaitForSeconds(
                Mathf.Lerp(
                    pathWave.waveData.spawnDelay.x,
                    pathWave.waveData.spawnDelay.y,
                    Random.Range(0f, 1f)
                ) * enemy.delayMult
            );
        }
    }

    private EnemyData PickAffordableEnemy(WaveData waveData, float budget)
    {
        List<EnemyData> affordable = new List<EnemyData>();

        foreach (var enemy in waveData.possibleEnemies)
        {
            if (enemy.spawnCost <= budget)
                affordable.Add(enemy);
        }

        if (affordable.Count == 0)
            return null;

        return affordable[Random.Range(0, affordable.Count)];
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
public class PathWave
{
    public Path path;
    public WaveData waveData;
}

[System.Serializable]
public class Wave
{
    [Tooltip("Delay before this wave starts")]
    public float startDelay = 10f;

    [Tooltip("Each path runs its own WaveData")]
    public PathWave[] pathWaves;
}