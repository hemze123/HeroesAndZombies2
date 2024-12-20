using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinDisplayUI : MonoBehaviour
{

    [SerializeField] private TMP_Text coinText;
    // Start is called before the first frame update
    
private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnIncreaseCoinUI, OnCoinIncreased);
        EventManager.AddHandler(GameEvent.OnDecreaseCoinUI, OnCoinDecreased);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnIncreaseCoinUI, OnCoinIncreased);
        EventManager.RemoveHandler(GameEvent.OnDecreaseCoinUI, OnCoinDecreased);
    }


     private void OnCoinIncreased()
    {
        UpdateCoinUI(Collector.Instance.GetCoinCount());
    }

    private void OnCoinDecreased()
    {
        UpdateCoinUI(Collector.Instance.GetCoinCount());
    }
    private void UpdateCoinUI(int coinValue)
    {
        coinText.text = "$" + coinValue.ToString();
    }

}
