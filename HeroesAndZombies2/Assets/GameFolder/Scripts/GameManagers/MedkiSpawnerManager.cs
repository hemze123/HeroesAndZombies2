using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitSpawnManager : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject[] medkitPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 15f;
    public int maxActiveMedkits = 3;

    private List<GameObject> activeMedkits = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnMedkitsRoutine());
    }

    IEnumerator SpawnMedkitsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (activeMedkits.Count < maxActiveMedkits)
            {
                SpawnMedkit();
            }
        }
    }

    void SpawnMedkit()
    {
        int pointIndex = Random.Range(0, spawnPoints.Length);
        int prefabIndex = Random.Range(0, medkitPrefabs.Length);

        GameObject medkit = Instantiate(medkitPrefabs[prefabIndex], spawnPoints[pointIndex].position, Quaternion.identity);
        activeMedkits.Add(medkit);

        medkit.GetComponent<Pickup>().onCollected += () => activeMedkits.Remove(medkit);
    }
}
