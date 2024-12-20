using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }

    private int currentHealth = 100;
    private int maxHealth = 100;
    private int coinCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    public void PlayerTakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        EventManager.Broadcast(GameEvent.OnDecreaseHealthUI, currentHealth);
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
            // Oyuncunun ölümü için gereken işlemleri buraya ekleyin.
        }
    }

    public void CollectMedKit(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        EventManager.Broadcast(GameEvent.OnIncreaseHealthUI, currentHealth);
        Debug.Log($"Player healed by {healAmount}. Current health: {currentHealth}");
    }

    public void CollectCoin(int value)
    {
        coinCount += value;
        EventManager.Broadcast(GameEvent.OnIncreaseCoinUI, coinCount);
    }
}
