using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int coins;

    public event Action<int> OnCoinsChanged; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        coins = SaveSystem.LoadCoins();
        OnCoinsChanged?.Invoke(coins);
    }

    public bool HasEnoughCoins(int amount) => coins >= amount;

    public void SpendCoins(int amount)
    {
        coins -= amount;
        SaveSystem.SaveCoins(coins);
        OnCoinsChanged?.Invoke(coins);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveSystem.SaveCoins(coins);
        OnCoinsChanged?.Invoke(coins);
    }

    public int GetCoins() => coins;
}
