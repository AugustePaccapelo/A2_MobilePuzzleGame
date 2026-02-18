using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{

    public GameObject creditsPanel = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject BoutonPause;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
{
    musicSlider.value = AudioManager.Instance.GetMusicVolume();
    sfxSlider.value = AudioManager.Instance.GetSFXVolume();

    musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
    sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
}


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        GameManager.Instance.StartGame();
    }

    public void ReloadLevel()
    {
        LevelManager.ReloadCurrentLevel();
        GameManager.Instance.LoadLevel();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenu != null) pauseMenu?.SetActive(true);
        if (BoutonPause != null) BoutonPause?.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null) pauseMenu?.SetActive(false);
        if (BoutonPause != null) BoutonPause?.SetActive(true);
    }

    public void ParaOpen()
    {
        pauseMenu.SetActive(false);
    }

    public void ParaClose()
    {
        pauseMenu.SetActive(true);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("SelectNiveau");
    }
    public void Credits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LeaveLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectNiveau");
    }
}
