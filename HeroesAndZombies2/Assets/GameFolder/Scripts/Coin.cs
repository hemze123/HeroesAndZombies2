using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class Coin : MonoBehaviour, ICollectible
{
    public int coinValue;
    public void CollectItem()
    {
        Collector.Instance.CollectCoin(coinValue);
        Debug.Log("Coin");
    }

    
}
