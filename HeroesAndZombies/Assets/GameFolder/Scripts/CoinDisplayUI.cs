using TMPro;
using UnityEngine;

public class CoinDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        EventManager.AddHandler<int>(GameEvent.OnIncreaseCoinUI, UpdateCoinUI);
        EventManager.AddHandler<int>(GameEvent.OnDecreaseCoinUI, UpdateCoinUI);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler<int>(GameEvent.OnIncreaseCoinUI, UpdateCoinUI);
        EventManager.RemoveHandler<int>(GameEvent.OnDecreaseCoinUI, UpdateCoinUI);
    }

    private void UpdateCoinUI(int coinValue)
    {
        coinText.text = $"${coinValue}";
    }
}
