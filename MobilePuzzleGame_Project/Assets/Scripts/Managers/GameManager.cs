using UnityEngine;

// Author : Auguste Paccapelo

public enum GameState
{
    NotInLevel,
    InitLevel,
    PlayerPlacingPlatforms,
    GamePlaying,
    EndingLevel
}

public class GameManager : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Singleton ----- \\

    public static GameManager Instance {get; private set;}

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    private static GameState _currentGameState = GameState.NotInLevel;
    public static GameState CurrentGameState => _currentGameState;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Debug.Log(nameof(GameManager) + " Instance already exist, destorying last added.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start() { }

    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            _currentGameState = GameState.GamePlaying;
        }
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}