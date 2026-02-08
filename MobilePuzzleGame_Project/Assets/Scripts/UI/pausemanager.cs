using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class pausemanager : MonoBehaviour
{
    public GameObject pausemenu;
    public Slider volumeslider;

    void Start()
    {
        pausemenu.SetActive(false);
        volumeslider.onValueChanged.AddListener(delegate { setvolume(); });
    }

    // Update is called once per frame
    void Update()
    {
        //displaytime();
    }

    public void pause()
    {
        Time.timeScale = 0;
        openmenu();
    }

    void openmenu()
    {
        Debug.Log("Menu Opened");
        pausemenu.SetActive(true);
    }

    public void closemenu()
    {
        if (GameManager.CurrentGameState == GameState.GamePlaying)
        {
            Time.timeScale = 1;
            pausemenu.SetActive(false);
        }
        else
        {
            pausemenu.SetActive(false);
        }
    }

    void savevolume()
    {
        PlayerPrefs.SetFloat("volume", volumeslider.value);
        PlayerPrefs.Save();
    }

    void setvolume()
    {
        AudioListener.volume = volumeslider.value;
        savevolume();
        Debug.Log("Volume set to: " + volumeslider.value);  
    }
}
