using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    public GameObject creditsPanel;
    public GameObject hamburgerMenu;
    void Start()
    {
        hamburgerMenu.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ContinueGame()
    {
        Debug.Log("Continue Game button clicked.");
        SceneManager.LoadScene("SampleScene");
    }
    public void LevelSelect()
    {
        Debug.Log("Level Select button clicked.");
        SceneManager.LoadScene("SelectNiveau");
    }
    public void Credits()
    {
        Debug.Log("Credits button clicked.");
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        Debug.Log("Close Credits button clicked.");
        creditsPanel.SetActive(false);
    }
    
    public void ReturnToMenu()
    {
        Debug.Log("Return to Menu button clicked.");
        SceneManager.LoadScene("MainMenu");
    }

    public void HamburgerMenu()
    {
        Debug.Log("Hamburger Menu button clicked.");
        hamburgerMenu.SetActive(!hamburgerMenu.activeSelf);
    }
}