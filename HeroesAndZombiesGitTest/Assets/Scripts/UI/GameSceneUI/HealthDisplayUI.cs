using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    private PlayerHealth playerHealth;

    public void Setup(PlayerHealth ph)
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealthUI;

        playerHealth = ph;
        playerHealth.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int current, int max)
    {
        healthBarSlider.value = max > 0 ? (float)current / max : 0f;
    }
}
