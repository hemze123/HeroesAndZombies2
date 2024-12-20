using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;

    private void OnEnable()
    {
        EventManager.AddHandler<int>(GameEvent.OnIncreaseHealthUI, UpdateHealthUI);
        EventManager.AddHandler<int>(GameEvent.OnDecreaseHealthUI, UpdateHealthUI);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler<int>(GameEvent.OnIncreaseHealthUI, UpdateHealthUI);
        EventManager.RemoveHandler<int>(GameEvent.OnDecreaseHealthUI, UpdateHealthUI);
    }

    private void UpdateHealthUI(int health)
    {
        int maxHealth = Collector.Instance.GetMaxHealth();
        healthBarSlider.value = (float)health / maxHealth;
    }
}
