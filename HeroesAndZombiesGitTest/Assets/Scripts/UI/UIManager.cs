using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Referansları")]
    [SerializeField] private GameObject storePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;

    public void ToggleStore()
    {
        AudioManager.Instance.PlayUIClick();

        if (storePanel != null && menuPanel != null)
        {
            bool isStoreActive = !storePanel.activeSelf;
            storePanel.SetActive(isStoreActive);
            menuPanel.SetActive(!isStoreActive);

            if (isStoreActive)
                StoreUI.Instance.LoadItems();
        }
        else
        {
            Debug.LogError("Panel referanslari eksik!");
        }
    }

    public void ToggleSettings()
    {
        AudioManager.Instance.PlayUIClick(); 

        if (settingsPanel == null || menuPanel == null)
        {
            Debug.LogError("Settings veya Menu panel referansi yoxdur!");
            return;
        }

        bool isSettingsActive = !settingsPanel.activeSelf;
        settingsPanel.SetActive(isSettingsActive);
        menuPanel.SetActive(!isSettingsActive);
    }

    public void OnPlayButtonClick()
    {
        AudioManager.Instance.PlayUIClick(); 
        GameManager.Instance.LoadGameScene();
    }
}
