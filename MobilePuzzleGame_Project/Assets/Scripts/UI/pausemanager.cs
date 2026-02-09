using UnityEngine;
using UnityEngine.UI;

public class pausemanager : MonoBehaviour
{
    public GameObject pausemenu;
    public Slider volumeslider;

    void Start()
    {
        if (pausemenu != null) pausemenu.SetActive(false);
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
        pausemenu.SetActive(true);
    }

    public void closemenu()
    {
        Time.timeScale = 1;
        pausemenu.SetActive(false);
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
    }
}
