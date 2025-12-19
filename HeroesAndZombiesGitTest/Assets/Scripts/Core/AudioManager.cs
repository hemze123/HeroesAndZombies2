using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("General Clips")]
    public AudioClip backgroundMusic;
    public AudioClip footstepClip;
    public AudioClip gunFireClip;
    public AudioClip zombieAttackClip;
    public AudioClip enemySpawnClip;

    [Header("UI Clips")]
    public AudioClip uiClickClip; 

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource footstepSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        footstepSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        footstepSource.loop = true;

        // Mixer 
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        footstepSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    //  MUSI
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    // SFX 
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    // FOOTSTEPS
    public void PlayFootsteps()
    {
        if (footstepClip == null || footstepSource.isPlaying) return;
        footstepSource.clip = footstepClip;
        footstepSource.Play();
    }

    public void StopFootsteps() => footstepSource.Stop();

    // UI
    public void PlayUIClick()
    {
        PlaySFX(uiClickClip);
    }

    //  VOLUME 
    public void SetMasterVolume(float value) =>
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);

    public void SetMusicVolume(float value) =>
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);

    public void SetSFXVolume(float value) =>
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);

    public void MuteAll(bool mute) => AudioListener.pause = mute;
}
