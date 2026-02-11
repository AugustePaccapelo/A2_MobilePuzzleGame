using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    private const string SAVE_KEY = "PlayerSaveData";
    private List<int> unlockedLevels = new List<int>();
    private int currentLevelIndex = 0;

    private void OnEnable() { }

    private void OnDisable()
    {
        SaveData();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    private void Start() { }

    private void Update() { }

    private void SaveData()
    {
        SaveDataClass data = new SaveDataClass();
        data.unlockedLevels = unlockedLevels;
        data.currentLevelIndex = currentLevelIndex;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            SaveDataClass data = JsonUtility.FromJson<SaveDataClass>(json);
            unlockedLevels = data.unlockedLevels;
            currentLevelIndex = data.currentLevelIndex;
        }
        else
        {
            unlockedLevels.Clear();
            unlockedLevels.Add(0);
            currentLevelIndex = 0;
            SaveData();
        }
    }

    public void UnlockLevel(int levelIndex)
    {
        if (!unlockedLevels.Contains(levelIndex))
        {
            unlockedLevels.Add(levelIndex);
            SaveData();
        }
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        return unlockedLevels.Contains(levelIndex);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        SaveData();
    }

    public int GetCurrentLevel()
    {
        return currentLevelIndex;
    }

    public List<int> GetUnlockedLevels()
    {
        return unlockedLevels;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}

[System.Serializable]
public class SaveDataClass
{
    public List<int> unlockedLevels = new List<int>();
    public int currentLevelIndex = 0;
}
