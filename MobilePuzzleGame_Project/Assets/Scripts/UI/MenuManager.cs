using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    public GameObject creditsPanel = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject BoutonPause;

    void Start()
    {
        if (creditsPanel != null) creditsPanel?.SetActive(false);
        if (pauseMenu != null) pauseMenu?.SetActive(false);
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
        LevelManager.Instance.BackToMenu();
    }
}
