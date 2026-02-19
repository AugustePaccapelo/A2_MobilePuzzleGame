using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public List<AudioSource> musicClips = new();
    public List<AudioSource> sfxClips = new();
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
}


    void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }
    
    public void SetMusicVolume(float volume)
{
    foreach (var clip in musicClips)
    {
        clip.volume = volume;
    }
    PlayerPrefs.SetFloat("MusicVolume", volume);
}

public void SetSFXVolume(float volume)
{
    foreach (var clip in sfxClips)
    {
        clip.volume = volume;
    }
    PlayerPrefs.SetFloat("SFXVolume", volume);
}

public float GetMusicVolume()
{
    return PlayerPrefs.GetFloat("MusicVolume", 1f);
}

public float GetSFXVolume()
{
    return PlayerPrefs.GetFloat("SFXVolume", 1f);
}

public void OnAnyButtonClicked()
{
    sfxClips[8].Play();
}


    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
    
}
