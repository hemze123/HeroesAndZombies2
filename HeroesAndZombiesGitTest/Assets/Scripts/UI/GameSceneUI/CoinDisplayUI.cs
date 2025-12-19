using TMPro;
using UnityEngine;

public class CoinDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoinUI;
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinUI;
    }

    private void UpdateCoinUI(int coinValue)
    {
        coinText.text = $"${coinValue}";
    }
}
