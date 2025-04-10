using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Düşman Prefabları")]
    public List<GameObject> enemyTiers; // 0 = kolay, 1 = orta, 2 = zor düşmanlar

    [Header("Wave Ayarları")]
    public float timeBetweenWaves = 10f;
    public int enemiesPerWave = 5;
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    public BossSpawnManager bossSpawnManager;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;
            int spawnCount = enemiesPerWave + currentWave * 2;

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.3f);
            }

            bossSpawnManager.SpawnBoss(currentWave);
        }
    }

    void SpawnEnemy()
    {
        int tierIndex = Mathf.Clamp(currentWave / 3, 0, enemyTiers.Count - 1);
        GameObject enemyPrefab = enemyTiers[tierIndex];
        Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemiesAlive++;

        enemy.GetComponent<Enemy>().onDeath += () => enemiesAlive--;
    }
}
