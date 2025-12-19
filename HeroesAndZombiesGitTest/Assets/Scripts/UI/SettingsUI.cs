using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager sehnede tapilmadi!");
            return;
        }

        // --- Listener ---
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        // --- save yükle ---
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value  = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value    = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // --- Panel x butonu ---
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        // --- save  ---
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
