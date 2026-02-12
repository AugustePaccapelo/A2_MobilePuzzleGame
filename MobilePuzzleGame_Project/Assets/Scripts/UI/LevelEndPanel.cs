using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEndPanel : MonoBehaviour
{
    public static LevelEndPanel Instance { get; private set; }
    public TextMeshProUGUI levelCompleteText;

    private int currentLevelIndex;

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update() { }

    public void ShowPanel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        gameObject.SetActive(true);

        if (levelCompleteText != null)
            levelCompleteText.text = "Niveau " + (levelIndex + 1) + " Complété!";

        Time.timeScale = 0f;
    }

    public void OnNextLevelClicked()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        PlayerData.Instance.UnlockLevel(currentLevelIndex + 1);
        LevelManager.Instance.LoadLevelByIndex(currentLevelIndex + 1);
    }

    public void OnMenuClicked()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        GameManager.Instance.QuitLevel();
        LevelManager.Instance.BackToMenu();
        Debug.Log("Retour au menu !");
    }

    private void OnDestroy() { }
}
