using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.OnPlayerDied -= ShowGameOver;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitAndSubscribePlayerHealth());
        isPaused = false;
        Time.timeScale = 1f;
        HideAll();
        if (hudPanel != null) hudPanel.SetActive(true);
    }

    private void Start()
    {
        HideAll();
        if (hudPanel != null) hudPanel.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(WaitAndSubscribePlayerHealth());
    }

    private System.Collections.IEnumerator WaitAndSubscribePlayerHealth()
    {
        
        int tries = 0;
        while (PlayerHealth.Instance == null && tries < 20)
        {
            yield return new WaitForSeconds(0.1f);
            tries++;
        }

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnPlayerDied -= ShowGameOver;
            PlayerHealth.Instance.OnPlayerDied += ShowGameOver;
            Debug.Log("GameUIManager: PlayerHealth event'ine qosuldu!");
        }
        else
        {
            Debug.LogWarning(" GameUIManager: PlayerHealth tapilmadi.");
        }
    }

    private void HideAll()
    {
        if (hudPanel != null) hudPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (gameOverPanel != null && gameOverPanel.activeSelf) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            HideAll();
            if (pausePanel != null) pausePanel.SetActive(true);
            Debug.Log("Oyun dayandi.");
        }
        else
        {
            Time.timeScale = 1f;
            HideAll();
            if (hudPanel != null) hudPanel.SetActive(true);
            Debug.Log("Oyun Devam Edir.");
        }
    }

    public void ShowGameOver()
    {
        if (isPaused) isPaused = false;

        HideAll();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log(" GameOver paneli acildi!");
        }
        else
        {
            Debug.LogError("GameOver panel referansi yoxdur");
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
{
    Time.timeScale = 1f;

    
    Scene currentScene = SceneManager.GetActiveScene();
    LoadingSceneController.LoadScene(currentScene.name);

    Debug.Log(" Oyun yeniden baslayir");
}

public void ReturnToMenu()
{
    Time.timeScale = 1f;

    // GameManager üzerinden menüye dön
    GameManager.Instance.ReturnToMenu();

    Debug.Log(" Menuya qayidir");
}

}
