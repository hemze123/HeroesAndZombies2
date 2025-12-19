using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore { get; private set; }
    public int bestScore { get; private set; }

    private const string BestScoreKey = "BestScore";

    // Event → UI buraya abone olmalidir
    public event Action<int, int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateBestScore();
        NotifyUI();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateBestScore();
        NotifyUI();
    }

    private void UpdateBestScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }
    }

    private void NotifyUI()
    {
        OnScoreChanged?.Invoke(currentScore, bestScore);
    }
}
