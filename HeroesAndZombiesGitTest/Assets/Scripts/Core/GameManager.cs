using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Game, Store }
    public GameState currentState;

    public FloatingJoystick moveJoystick;
    public FloatingJoystick lookJoystick;

    
    public System.Action OnJoysticksFound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        DataManager.Instance.LoadAllItemData();
        currentState = GameState.Menu;
        LoadingSceneController.LoadScene("Menu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentState == GameState.Game)
        {
            FindJoysticksInScene();
            
            if (OnJoysticksFound != null)
            {
                OnJoysticksFound.Invoke();
            }
        }
    }

    private void FindJoysticksInScene()
    {
        GameObject moveJoystickObject = GameObject.FindWithTag("MoveJoystick");
        GameObject lookJoystickObject = GameObject.FindWithTag("LookJoystick");

        if (moveJoystickObject != null)
        {
            moveJoystick = moveJoystickObject.GetComponent<FloatingJoystick>();
        }
        else
        {
            moveJoystick = null; // 
            Debug.LogWarning("Move Joystick sehnede tapilmadi.");
        }

        if (lookJoystickObject != null)
        {
            lookJoystick = lookJoystickObject.GetComponent<FloatingJoystick>();
        }
        else
        {
            lookJoystick = null; 
            Debug.LogWarning("Look Joystick sahnede tapilmadi.");
        }
    }

    public void LoadGameScene()
    {
        currentState = GameState.Game;
        string selectedLevelSceneName = SaveSystem.LoadSelectedItem(ItemBaseSO.ItemType.Level);

        if (string.IsNullOrEmpty(selectedLevelSceneName) || !DataManager.Instance.LevelExists(selectedLevelSceneName))
        {
            LoadingSceneController.LoadScene(DataManager.Instance.GetDefaultLevel().sceneName);
        }
        else
        {
            LoadingSceneController.LoadScene(selectedLevelSceneName);
        }
    }

    public void ReturnToMenu()
    {
        currentState = GameState.Menu;
         LoadingSceneController.LoadScene("Menu");
    }
}