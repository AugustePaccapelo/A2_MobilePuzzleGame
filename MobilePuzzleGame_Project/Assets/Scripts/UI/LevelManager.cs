using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform buttonContainer;
    public string levelFolder = "Levels";
    private List<Button> levelButtons = new List<Button>();
    public GameObject UINiveau;
    public GameObject UIMenu;
    public ScrollRect scrollbar;

    private string currentLevelPrefabName;
    private GameObject currentLevelInstance;

    void Start()
    {
        UINiveau.SetActive(false);
        UIMenu.SetActive(true);

        var gridLayout = buttonContainer.GetComponent<UnityEngine.UI.GridLayoutGroup>();
        if (gridLayout == null) buttonContainer.gameObject.AddComponent<UnityEngine.UI.GridLayoutGroup>();
        var layout = buttonContainer.GetComponent<UnityEngine.UI.GridLayoutGroup>();
        layout.cellSize = new Vector2(300, 300);
        var spacing = 20;
        layout.spacing = new Vector2(spacing, spacing);
        var center = TextAnchor.UpperCenter;
        layout.childAlignment = center;
        var containerAdapter = buttonContainer.GetComponent<ContentSizeFitter>();
        if (containerAdapter == null) buttonContainer.gameObject.AddComponent<ContentSizeFitter>();
        var fitter = buttonContainer.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        LoadLevelButtons();
    }

    void Update()
    {

    }

    void LoadLevelButtons()
{
    levelButtons.Clear();
    foreach (Transform child in buttonContainer)
        Destroy(child.gameObject);

    GameObject[] levelPrefabs = Resources.LoadAll<GameObject>(levelFolder);
    for (int i = 0; i < levelPrefabs.Length; i++)
    {
        GameObject prefab = levelPrefabs[i];
        GameObject btnObj = Instantiate(levelButtonPrefab, buttonContainer, false);
        Button btn = btnObj.GetComponent<Button>();
        Text txt = btnObj.GetComponentInChildren<Text>();
        if (txt != null) txt.text = prefab.name;

        string prefabName = prefab.name;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => LoadLevelByName(prefabName));

        levelButtons.Add(btn);
    }

    Canvas.ForceUpdateCanvases();
    if (scrollbar != null)
        scrollbar.verticalNormalizedPosition = 1f;
}

public void LoadLevelByName(string prefabName)
{
    string path = levelFolder + "/" + prefabName;
    GameObject levelPrefab = Resources.Load<GameObject>(path);
    if (levelPrefab != null)
    {
        if (currentLevelInstance != null)
            Destroy(currentLevelInstance);

        currentLevelInstance = Instantiate(levelPrefab);
        UINiveau.SetActive(true);
        UIMenu.SetActive(false);
        currentLevelPrefabName = prefabName;
    }
}


    public void UnlockLevel(int levelID)
    {
        if(levelID < levelButtons.Count)
        {
            levelButtons[levelID].interactable = true;
        }
    }

    public void LockLevel(int levelID)
    {
        if(levelID < levelButtons.Count)
        {
            levelButtons[levelID].interactable = false;
        }
    }

    public void BackToMenu()
    {
        UINiveau.SetActive(false);
        UIMenu.SetActive(true);
    }
    public void ReloadCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }
        if (!string.IsNullOrEmpty(currentLevelPrefabName))
        {
            string path = levelFolder + "/" + currentLevelPrefabName;
            GameObject levelPrefab = Resources.Load<GameObject>(path);
            if (levelPrefab != null)
            {
                if (currentLevelInstance != null)
                    Destroy(currentLevelInstance);

                currentLevelInstance = Instantiate(levelPrefab);
            }
    }
}
}
