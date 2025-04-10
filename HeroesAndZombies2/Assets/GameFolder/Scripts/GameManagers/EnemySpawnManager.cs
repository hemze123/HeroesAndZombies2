using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyTier
    {
        public string name;
        public List<GameObject> enemyPrefabs; // Bu seviyeye ait düşman prefabları
        public float minSpawnTime = 3f;
        public float maxSpawnTime = 8f;
        public int maxEnemiesAtOnce = 5;
        public int minLevelToSpawn = 0; // Bu düşman seviyesinin ortaya çıkacağı minimum oyun seviyesi
    }

    public List<Transform> spawnPoints; // Spawn noktaları (Inspector’dan ayarlanabilir)
    public List<EnemyTier> enemyTiers;  // Seviye bazlı düşman listesi
    public float spawnCheckInterval = 2f; // Kaç saniyede bir düşman spawn edilmesi kontrol edilir
    public int currentLevel = 0; // Oyun seviyesi (zamanla artabilir)
    public float levelUpInterval = 30f; // Her 30 saniyede bir seviye artar

    private float nextLevelUpTime;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        nextLevelUpTime = Time.time + levelUpInterval;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        // Zaman geçtikçe seviye artır
        if (Time.time >= nextLevelUpTime)
        {
            currentLevel++;
            nextLevelUpTime = Time.time + levelUpInterval;
            Debug.Log("Level Up! Current Level: " + currentLevel);
        }

        // Spawn edilmiş düşman listesini temizle
        spawnedEnemies.RemoveAll(item => item == null);
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemyIfPossible();
            yield return new WaitForSeconds(spawnCheckInterval);
        }
    }

    void SpawnEnemyIfPossible()
    {
        // Aktif düşman sınırını aşma
        int totalEnemies = spawnedEnemies.Count;
        int maxTotal = 0;

        foreach (var tier in enemyTiers)
        {
            if (currentLevel >= tier.minLevelToSpawn)
                maxTotal += tier.maxEnemiesAtOnce;
        }

        if (totalEnemies >= maxTotal)
            return;

        // Uygun tier bul
        List<EnemyTier> validTiers = enemyTiers.FindAll(t => currentLevel >= t.minLevelToSpawn);
        if (validTiers.Count == 0) return;

        EnemyTier selectedTier = validTiers[Random.Range(0, validTiers.Count)];

        if (selectedTier.enemyPrefabs.Count == 0 || spawnPoints.Count == 0) return;

        GameObject prefabToSpawn = selectedTier.enemyPrefabs[Random.Range(0, selectedTier.enemyPrefabs.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject spawned = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(spawned);
    }
}
