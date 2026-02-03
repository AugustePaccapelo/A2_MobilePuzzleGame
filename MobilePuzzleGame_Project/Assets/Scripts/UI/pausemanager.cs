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
        Debug.Log("Menu Closed");
        pausemenu.SetActive(false);
        Time.timeScale = 1;
    }

    void displaytime()
    {
        float currentTime = Time.timeSinceLevelLoad;
        Debug.Log("Current Time: " + currentTime.ToString("F2") + "s");
        
    }

    void setvolume()
    {
        AudioListener.volume = volumeslider.value;
        Debug.Log("Volume set to: " + volumeslider.value);  
    }
}
