using UnityEngine;

public class MedKit : MonoBehaviour, ICollectible
{
    [SerializeField] private int healAmount = 25;

    public void CollectItem()
    {
        PlayerHealth.Instance.Heal(healAmount);
        Destroy(gameObject);
    }
}
