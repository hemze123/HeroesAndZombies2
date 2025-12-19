using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public static event Action OnPlayerSpawned; // Oyuncu spawn  event

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    [SerializeField] private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        private set => currentHealth = value;
    }

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDied;

    private void Awake()
    {
        Instance = this;
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Start()
    {
        
        OnPlayerSpawned?.Invoke();
        Debug.Log("✅ PlayerHealth: Oyuncu spawn edildi!");
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        Debug.Log("Player took damage! HP = " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead! OnPlayerDied event tetiklendi.");
        OnPlayerDied?.Invoke();

        // Player hereketi bagla
        var pm = GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        var ws = GetComponent<WeaponSystem>();
        if (ws != null) ws.enabled = false;
    }

    public void Heal(int amount)
    {
        if (CurrentHealth <= 0) return; // olu oyuncu  saglmaz.

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}
