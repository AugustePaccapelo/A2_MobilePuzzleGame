using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelEndPanel : MonoBehaviour
{
    public static LevelEndPanel Instance { get; private set; }
    [SerializeField] GameObject menuPanel;
    public TextMeshProUGUI levelCompleteText;
    public bool Onmenu = false;

    private int currentLevelIndex;

    private void OnEnable() { }

    private void OnDisable()
    {
    }

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
        Onmenu = true;
    }

    private void Update() { }

    public void ShowPanel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        gameObject.SetActive(true);
        Onmenu = false;

        if (levelCompleteText != null)
            levelCompleteText.text = "Niveau " + (levelIndex + 1);

    }

    public void OnNextLevelClicked()
    {
        gameObject.SetActive(false);
        PlayerData.UnlockLevel(currentLevelIndex + 1);
        LevelManager.Instance.LoadLevelByIndex(currentLevelIndex + 1);
    }

    public void OnMenuClicked()
    {
        GameManager.Instance.LoadLevel();
        SceneManager.LoadScene("SelectNiveau");
        Debug.Log("Retour au menu !");

    }

    private void OnDestroy() { }
}
