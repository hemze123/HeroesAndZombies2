using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private FireButtonUI fireButtonUI;
    [SerializeField] private HealthDisplayUI healthDisplayUI;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private List<Transform> weaponSpawnPoints;

    [Header("Prefabs")]
    [SerializeField] private GameObject medkitPrefab;

    [Header("Waves")]
    [SerializeField] private List<WaveConfig> waves;
    private int currentWaveIndex = 0;
    private bool isSpawningEnemies = false;

    private GameObject player;

    private void Start()
    {
        SpawnPlayer();
        SpawnInitialWeapons();
        StartCoroutine(DelayedWaveStart());
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnJoysticksFound += AssignJoysticksToPlayer;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnJoysticksFound -= AssignJoysticksToPlayer;
    }

    #region Player
    private void SpawnPlayer()
    {
        CharacterItemSO character = DataManager.Instance.GetSelectedCharacter();
        player = Instantiate(character.characterPrefab, playerSpawnPoint.position, Quaternion.identity);

        if (player.TryGetComponent(out PlayerController controller) && fireButtonUI != null)
            fireButtonUI.BindToPlayer(controller);

        if (player.TryGetComponent(out PlayerHealth health) && healthDisplayUI != null)
            healthDisplayUI.Setup(health);

        var vcam = FindAnyObjectByType<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.Follow = player.transform;
            Debug.Log("[SpawnManager] Player Cinemachine'e bağlandi.");
        }
    }

    private void AssignJoysticksToPlayer()
    {
        if (player == null) return;
        if (player.TryGetComponent(out PlayerMovement movement))
        {
            movement.moveJoystick = GameManager.Instance.moveJoystick;
            movement.lookJoystick = GameManager.Instance.lookJoystick;
        }
    }
    #endregion

    #region Weapons
    private void SpawnInitialWeapons()
    {
        foreach (Transform spawn in weaponSpawnPoints)
            SpawnRandomWeapon(spawn.position);
    }

    private void SpawnRandomWeapon(Vector3 position)
    {
        var unlocked = DataManager.Instance.GetUnlockedWeapons();
        var defaultWeapon = DataManager.Instance.GetDefaultWeapon();
        var available = unlocked.FindAll(w => w != defaultWeapon);
        if (available.Count == 0) return;

        WeaponItemSO randomWeapon = available[Random.Range(0, available.Count)];
        Instantiate(randomWeapon.weaponPrefab, position, Quaternion.identity);
    }
    #endregion

    #region Waves
    private IEnumerator WaveRoutine()
{
    while (currentWaveIndex < waves.Count)
    {
        WaveConfig wave = waves[currentWaveIndex];

        // Her dalga baslamadan önce gozleme vaxti
        if (wave.waveStartDelay > 0)
        {
            Debug.Log($"[Wave] {currentWaveIndex + 1}. dalga {wave.waveStartDelay} saniye sonra başlayacak...");
            yield return new WaitForSeconds(wave.waveStartDelay);
        }

        Debug.Log($"[Wave] {currentWaveIndex + 1}. dalga başladi!");
        
        StartCoroutine(WeaponRespawnRoutine(wave.weaponRespawnTime));
        yield return StartCoroutine(SpawnEnemiesWithLogic(wave));

        Debug.Log($"[Wave] {currentWaveIndex + 1}. dalga bitti!");

        currentWaveIndex++;
        yield return new WaitForSeconds(3f); // İki dalga arası gozleme zamani
    }

    Debug.Log("[Wave] Tüm dalgalar tamamlandi!");
}

    private IEnumerator SpawnEnemiesWithLogic(WaveConfig wave)
    {
        isSpawningEnemies = true;
        int totalSpawned = 0;

        List<Transform> shuffledPoints = new List<Transform>(enemySpawnPoints);

        void ShuffleSpawnPoints()
        {
            for (int i = 0; i < shuffledPoints.Count; i++)
            {
                Transform temp = shuffledPoints[i];
                int randomIndex = Random.Range(i, shuffledPoints.Count);
                shuffledPoints[i] = shuffledPoints[randomIndex];
                shuffledPoints[randomIndex] = temp;
            }
        }

        while (totalSpawned < wave.enemiesPerWave)
        {
            ShuffleSpawnPoints();

            int spawnCount = wave.spawnInBatches
                ? Mathf.Min(wave.enemiesPerBatch, wave.enemiesPerWave - totalSpawned)
                : 1;

            for (int i = 0; i < spawnCount; i++)
            {
                Transform spawnPoint = shuffledPoints[i % shuffledPoints.Count];
                GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];

                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                // 🔹 Medkit spawn ihtimali
                if (Random.value < wave.medkitSpawnChance)
                {
                    Vector3 offset = spawnPoint.position + new Vector3(Random.Range(1f, 2f), 0, Random.Range(1f, 2f));
                    Instantiate(medkitPrefab, offset, Quaternion.identity);
                }

                totalSpawned++;
            }

            float interval = wave.spawnInBatches ? wave.batchInterval : wave.spawnInterval;
            yield return new WaitForSeconds(interval);
        }

        isSpawningEnemies = false;
    }
    private IEnumerator DelayedWaveStart()
{
    Debug.Log("[SpawnManager] Oyun başlatildi, düşman spawn'i birazdan başlayacak...");

    float delayBeforeWaves = 5f; //
    yield return new WaitForSeconds(delayBeforeWaves);

    Debug.Log("[SpawnManager] Wave sistemi başlatiliyor...");
    yield return StartCoroutine(WaveRoutine());
}


    private IEnumerator WeaponRespawnRoutine(float respawnTime)
    {
        while (isSpawningEnemies)
        {
            yield return new WaitForSeconds(respawnTime);
            Debug.Log("[SpawnManager] Weapon respawn başlatiliyor...");
            SpawnInitialWeapons();
        }
    }
    #endregion
}
