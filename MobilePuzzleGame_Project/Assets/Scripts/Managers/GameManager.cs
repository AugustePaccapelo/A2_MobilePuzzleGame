using System;
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

    // ----- Events ----- \\

    static public event Action onGameStart;

    // ----- Others ----- \\

    private static GameState _currentGameState = GameState.NotInLevel;
    public static GameState CurrentGameState => _currentGameState;

    private Action _currentSate;

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

    void Start()
    {
        InitNotInLevel();
    }

    void Update()
    {
        _currentSate?.Invoke();
    }

    // ----- My Functions ----- \\

    public void LoadLevel()
    {
        InitInitatingLevel();
    }

    public void StartGame()
    {
        InitGamePlaying();
    }

    public void FinishLevel()
    {
        InitGameEnding();
    }

    private void InitNotInLevel()
    {
        _currentGameState = GameState.NotInLevel;
        _currentSate = NotInLevel;
    }

    private void NotInLevel()
    {

    }

    private void InitInitatingLevel()
    {
        _currentGameState = GameState.InitLevel;
        _currentSate = InitatingLevel;
    }

    private void InitatingLevel()
    {
        InitPlayerPlacingPlatform();
    }

    public void InitPlayerPlacingPlatform()
    {
        _currentGameState = GameState.PlayerPlacingPlatforms;
        _currentSate = PlayerPlacingPlatform;
    }

    private void PlayerPlacingPlatform()
    {
        
    }

    private void InitGamePlaying()
    {
        _currentGameState = GameState.GamePlaying;
        _currentSate = GamePlaying;
        onGameStart?.Invoke();
    }

    private void GamePlaying()
    {

    }

    private void InitGameEnding()
    {
        _currentGameState = GameState.EndingLevel;
        _currentSate = GameEnding;
    }

    private void GameEnding()
    {
        Debug.Log("Game endend !");
        
        int currentLevel = PlayerData.Instance.GetCurrentLevel();
        PlayerData.Instance.UnlockLevel(currentLevel + 1);
        
        if (LevelEndPanel.Instance != null)
            LevelEndPanel.Instance.ShowPanel(currentLevel);
    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}