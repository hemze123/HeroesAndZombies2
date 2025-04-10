using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Noktaları")]
    public Transform[] weaponSpawnPoints;
    public Transform[] medkitSpawnPoints;
    public Transform[] enemySpawnPoints;

    [Header("Prefablar")]
    public GameObject[] weaponPrefabs;
    public GameObject[] enemyPrefabs; // Level'a göre farklı düşmanlar
    public GameObject bossPrefab;
    public GameObject medkitPrefab;

    [Header("Ayarlar")]
    public float spawnInterval = 5f;
    public int maxMedkits = 3;
    public int enemiesPerWave = 5;
    public float difficultyRamp = 1.2f;

    private List<GameObject> activeMedkits = new();
    private int waveNumber = 1;
    private int enemiesAlive = 0;

    private void Start()
    {
        StartCoroutine(WeaponSpawner());
        StartCoroutine(MedkitSpawner());
        StartCoroutine(WaveSpawner());
    }

    // SİLAH SPAWNER
    private IEnumerator WeaponSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Transform spawnPoint = weaponSpawnPoints[Random.Range(0, weaponSpawnPoints.Length)];
            GameObject prefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // MEDKIT SPAWNER
    private IEnumerator MedkitSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (activeMedkits.Count < maxMedkits)
            {
                Transform spawnPoint = medkitSpawnPoints[Random.Range(0, medkitSpawnPoints.Length)];
                GameObject medkit = Instantiate(medkitPrefab, spawnPoint.position, Quaternion.identity);
                activeMedkits.Add(medkit);

                medkit.GetComponent<Pickup>().onCollected += () => activeMedkits.Remove(medkit);
            }
        }
    }

    // WAVE SPAWNER
    private IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            StartCoroutine(SpawnWave());
            waveNumber++;
            enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * difficultyRamp);
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            yield return new WaitForSeconds(1f);

            Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            GameObject prefab = GetEnemyForCurrentWave();
            GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;

            enemy.GetComponent<Enemy>().onDeath += () => enemiesAlive--;
        }

        // Boss her 5 dalgada bir gelir
        if (waveNumber % 5 == 0)
        {
            yield return new WaitForSeconds(2f);
            Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;

            boss.GetComponent<Enemy>().onDeath += () => enemiesAlive--;
        }
    }

    private GameObject GetEnemyForCurrentWave()
    {
        int index = Mathf.Min(waveNumber / 2, enemyPrefabs.Length - 1);
        return enemyPrefabs[index];
    }
}
