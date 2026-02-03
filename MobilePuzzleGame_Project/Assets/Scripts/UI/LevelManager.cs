using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Assigner les prefabs niv ici")]
    public List<GameObject> levelPrefabs = new List<GameObject>();
    public Transform levelParent; 

    private GameObject currentLevelInstance;
    private int currentIndex = 0;
    private const string PlayerPrefKey = "CurrentLevelIndex";
    public GameObject UISelect;
    public GameObject UILevel;
    private bool isLevelUnlocked = false; 

    void Start()
    {
        UILevel.SetActive(false);
    }

    public void LoadLevel(int index)
    {
        /*if (!isLevelUnlocked)
        {
            Debug.Log("Level is locked. Cannot load level.");
            return;
        }*/
        UISelect.SetActive(false);
        UILevel.SetActive(true);
        if (levelPrefabs == null || levelPrefabs.Count == 0) return;
        index = Mathf.Clamp(index, 0, levelPrefabs.Count - 1);

        if (currentLevelInstance != null) Destroy(currentLevelInstance);

        currentLevelInstance = Instantiate(levelPrefabs[index], levelParent, isLevelUnlocked);
        currentIndex = index;
        PlayerPrefs.SetInt(PlayerPrefKey, currentIndex);
        PlayerPrefs.Save();
    }

    public void ReloadLevel()
    {
        LoadLevel(currentIndex);
    }

    public void LoadNextLevel()
    {
        int next = currentIndex + 1;
        if (next < levelPrefabs.Count) LoadLevel(next);
        else Debug.Log("No next level available.");
    }  

    public void BackToMenu()
    {
        UISelect.SetActive(true);
        UILevel.SetActive(false);
        if (currentLevelInstance != null) Destroy(currentLevelInstance);
    }
    
}
