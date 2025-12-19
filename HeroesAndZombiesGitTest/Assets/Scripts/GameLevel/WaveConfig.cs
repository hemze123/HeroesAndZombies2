using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Game/Wave Config", order = 1)]
public class WaveConfig : ScriptableObject
{
    [Header("Wave Settings")]
    public int enemiesPerWave = 3;          // Bu dalgada spawn olacak dusman sayısı
    public float spawnInterval = 5f;        // Spawn arası saniye

    public GameObject[] enemyPrefabs;       // Bu dalgada  dusman tipleri
     [Header("Wave Timing")]
    public float waveStartDelay = 5f; //  Dalga başlamadan once gozleme vaxti

    [Header("Batch Spawn")]
    public bool spawnInBatches = false;  //grup mu spawn edilecek?
    public int enemiesPerBatch = 3;  // Bir seferde nece  düşman spawn edilecek?
    public float batchInterval = 2f; // Batch arası sure (saniye)


    [Header("Medkit Settings")]
    [Range(0f, 1f)]
    public float medkitSpawnChance = 0.1f;  // Medkit spawn faizi

    [Header("Weapon Respawn Settings")]
    public float weaponRespawnTime = 30f;   // Silahların spawn vaxti (saniye)
}
