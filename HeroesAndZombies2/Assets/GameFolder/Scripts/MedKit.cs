
using UnityEngine;

public class MedKit : MonoBehaviour, ICollectible
{
    [SerializeField] private int healAmount = 25;

    public void CollectItem()
    {
        Collector.Instance.CollectMedKit(healAmount);
        Destroy(gameObject); // MedKit toplandıktan sonra yok edilir.
    }
}
