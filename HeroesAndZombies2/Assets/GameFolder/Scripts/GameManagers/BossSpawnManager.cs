using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BossSpawnManager : MonoBehaviour
{
    [Header("Boss Prefabs")]
    public GameObject miniBossPrefab;
    public GameObject bossPrefab;

    [Header("Boss Ayarları")]
    public int miniBossWaveInterval = 5;
    public int bossWaveInterval = 10;
    public Transform[] bossSpawnPoints;

    public void SpawnBoss(int currentWave)
    {
        if (currentWave % bossWaveInterval == 0)
        {
            Instantiate(bossPrefab, GetRandomSpawnPoint(), Quaternion.identity);
        }
        else if (currentWave % miniBossWaveInterval == 0)
        {
            Instantiate(miniBossPrefab, GetRandomSpawnPoint(), Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return bossSpawnPoints[Random.Range(0, bossSpawnPoints.Length)].position;
    }
}
