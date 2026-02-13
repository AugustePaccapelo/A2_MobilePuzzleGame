using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public GameObject levelButtonPrefab;
    public Transform buttonContainer;
    static public string levelFolder = "Levels";
    private List<Button> levelButtons = new List<Button>();
    public GameObject UIMenu;
    public ScrollRect scrollbar;

    static private string currentLevelPrefabName;
    static private GameObject currentLevelInstance;

    static private Vector2 _baseResolution = new Vector2(1080, 1920);

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

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
        TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        //if (txt != null) txt.text = prefab.name;

        bool isUnlocked = PlayerData.Instance.IsLevelUnlocked(i);
        btn.interactable = isUnlocked;

        string prefabName = prefab.name;
        int levelIndex = i;
        txt.text = (levelIndex + 1).ToString();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => LoadLevelByName(prefabName));
        btn.onClick.AddListener(() => PlayerData.Instance.SetCurrentLevel(levelIndex));

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
        ScaleLevel(currentLevelInstance);
        UIMenu.SetActive(false);
        currentLevelPrefabName = prefabName;
        GameManager.Instance.LoadLevel();
    }
}

public void LoadLevelByIndex(int levelIndex)
{
    GameObject[] levelPrefabs = Resources.LoadAll<GameObject>(levelFolder);
    if (levelIndex >= 0 && levelIndex < levelPrefabs.Length)
    {
        if (PlayerData.Instance != null)
            PlayerData.Instance.SetCurrentLevel(levelIndex);

        LoadLevelByName(levelPrefabs[levelIndex].name);
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
        UIMenu.SetActive(true);
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
            currentLevelPrefabName = null;
        }
    }
    static public void ReloadCurrentLevel()
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
                {
                    currentLevelInstance.SetActive(false);
                    Destroy(currentLevelInstance);
                }

                currentLevelInstance = Instantiate(levelPrefab);
                ScaleLevel(currentLevelInstance);
            }
        }
    }

    static private void ScaleLevel(GameObject level)
    {
        float ratioX = _baseResolution.x / Screen.width;
        float ratioY = _baseResolution.y / Screen.height;

        float ratio = Mathf.Min(ratioX, ratioY);
        //Debug.Log(ratioX + "    " + ratioY);
        level.transform.localScale *= ratio;
    }
}
