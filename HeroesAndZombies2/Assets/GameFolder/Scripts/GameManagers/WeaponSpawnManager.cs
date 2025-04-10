using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnManager : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public List<GameObject> weaponPrefabs; // Spawn edilecek silah prefabları
    public List<Transform> spawnPoints;    // Silahların spawn olabileceği noktalar

    [Header("Zaman Ayarları")]
    public float spawnIntervalMin = 10f;  // Minimum spawn süresi
    public float spawnIntervalMax = 20f;  // Maksimum spawn süresi

    [Header("Başlangıçta spawn?")]
    public bool spawnOnStart = true;

    [Header("Tekrarlı spawn?")]
    public bool continuousSpawning = true;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnWeapon(); // Oyun başında bir silah spawnla
        }

        if (continuousSpawning)
        {
            StartCoroutine(SpawnWeaponRoutine());
        }
    }

    IEnumerator SpawnWeaponRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);
            SpawnWeapon();
        }
    }

    public void SpawnWeapon()
    {
        if (weaponPrefabs.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("WeaponSpawnManager: Silah prefabları veya spawn noktaları eksik.");
            return;
        }

        GameObject weaponToSpawn = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject spawnedWeapon = Instantiate(weaponToSpawn, spawnPoint.position, Quaternion.identity);
        spawnedWeapon.tag = "Weapon"; // Oyuncu çarpınca alabilmesi için
    }
}
