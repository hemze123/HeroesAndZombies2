using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayUI : MonoBehaviour
{

    [SerializeField] private Slider healthBarSlider;
   
      private void OnEnable() {
        EventManager.AddHandler(GameEvent.OnIncreaseHealthUI, OnHealthIncreased);
        EventManager.AddHandler(GameEvent.OnDecreaseHealthUI, OnHealthDecreased);
        
     }


     private void OnDisable() {
      
            EventManager.RemoveHandler(GameEvent.OnIncreaseHealthUI, OnHealthIncreased);
            EventManager.RemoveHandler(GameEvent.OnDecreaseHealthUI, OnHealthDecreased);
        
     }


     

     private void OnHealthIncreased() 
    {
        // Can arttırma olayı tetiklendiğinde bu metod çağrılır.
        UpdateUI(Collector.Instance.GetCurrentHealth()); // Güncel can değerini alıp arayüzü güncelle
    }
    private void OnHealthDecreased()
    {
        UpdateUI(Collector.Instance.GetCurrentHealth());
    }

    private void UpdateUI(int health)
    {
        int maxHealth = Collector.Instance.GetMaxHealth();
        healthBarSlider.value = ((float)health) / maxHealth;
    }

   
}
