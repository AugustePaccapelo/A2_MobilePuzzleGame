using UnityEngine;
using UnityEngine.UI;

public class paramanager : MonoBehaviour
{
    public GameObject paramenu;
    public Slider volumeslider;

    void Start()
    {
        if (paramenu != null)
        {
            paramenu.SetActive(false);
            volumeslider.onValueChanged.AddListener(delegate { setvolume(); });
        }       
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
        paramenu.SetActive(true);
    }

    public void closemenu()
    {
        paramenu.SetActive(false);
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
