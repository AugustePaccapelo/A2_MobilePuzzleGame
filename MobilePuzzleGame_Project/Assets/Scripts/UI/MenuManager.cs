using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    public GameObject creditsPanel = null;
    [SerializeField] private GameObject _burgerMenu = null;

    void Start()
    {
        if (creditsPanel != null) creditsPanel?.SetActive(false);
        if (_burgerMenu != null) _burgerMenu?.SetActive(false);
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

    public void ToggleBurgerMenu()
    {
        _burgerMenu.SetActive(!_burgerMenu.activeSelf);
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
