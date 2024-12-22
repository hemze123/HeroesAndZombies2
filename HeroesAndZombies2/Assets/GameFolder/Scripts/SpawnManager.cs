using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;  // Düşman prefabları
    [SerializeField] private GameObject[] itemPrefabs;  // Silah ve medkit prefabları
    [SerializeField] private Transform[] spawnPoints;  // Spawn noktaları
    [SerializeField] private float spawnInterval = 5.0f;  // Düşman spawn aralığı
    [SerializeField] private float itemSpawnInterval = 10.0f;  // Silah ve medkit spawn aralığı

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(itemSpawnInterval);
            SpawnItem();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        GameObject enemy = Instantiate(enemyPrefabs[randomEnemyIndex], 
                                       spawnPoints[randomSpawnIndex].position, 
                                       Quaternion.identity);
        Debug.Log($"Spawned {enemy.name} at {spawnPoints[randomSpawnIndex].position}");
    }

    private void SpawnItem()
    {
        if (itemPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        int randomItemIndex = Random.Range(0, itemPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        GameObject item = Instantiate(itemPrefabs[randomItemIndex], 
                                      spawnPoints[randomSpawnIndex].position, 
                                      Quaternion.identity);
        Debug.Log($"Spawned {item.name} at {spawnPoints[randomSpawnIndex].position}");
    }
}