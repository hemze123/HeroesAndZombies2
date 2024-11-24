using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
   public static Collector Instance { get; private set; }
   
    

    [Header("Setting")]
    [SerializeField]private int coins= 0;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 100;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
         EventManager.Broadcast(GameEvent.OnIncreaseHealthUI);
         EventManager.Broadcast(GameEvent.OnIncreaseCoinUI);  
    }


    public void CollectCoin(int amount)
    {
        coins += amount;
         EventManager.Broadcast(GameEvent.OnIncreaseCoinUI);
        
    }

    public void UpgradeWeapon(int amount)
    {
        coins -= amount;
       EventManager.Broadcast(GameEvent.OnDecreaseCoinUI);
    }

    public void CollectMedKit(int amount)
    {
         currentHealth += amount;
         currentHealth = Mathf.Min(currentHealth,100);
         EventManager.Broadcast(GameEvent.OnIncreaseHealthUI);
    }


    public void PlayerTakeDamage(int amount)
    {
       currentHealth -= amount;
       currentHealth = Mathf.Max(currentHealth,0);
       EventManager.Broadcast(GameEvent.OnDecreaseHealthUI);
    }

     public int GetCurrentHealth(){
        return currentHealth;
     }


    public int GetMaxHealth()
    {
        return maxHealth;
    }

      public int GetCoinCount()
    {
        return coins;
    }

}
