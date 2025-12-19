using UnityEngine;
using TMPro;

public class UI_ScoreUpdater : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI currentScoreText; 
    public TextMeshProUGUI bestScoreText;    

    private void OnEnable()
    {
        if (ScoreManager.Instance != null)
        {
           
            ScoreManager.Instance.OnScoreChanged += UpdateUI;

           
            UpdateUI(ScoreManager.Instance.currentScore, ScoreManager.Instance.bestScore);
        }
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            
            ScoreManager.Instance.OnScoreChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int current, int best)
    {
        if (currentScoreText != null)
            currentScoreText.text = "Score: " + current;

        if (bestScoreText != null)
            bestScoreText.text = "Best: " + best;
    }
}
