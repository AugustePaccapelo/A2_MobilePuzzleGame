using System;
using UnityEngine;

// Author : Auguste Paccapelo

public enum GameState
{
    NotInLevel,
    InitLevel,
    InGame,
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
    static public event Action onGameRestart;

    // ----- Others ----- \\

    private static GameState _currentGameState = GameState.NotInLevel;
    public static GameState CurrentGameState => _currentGameState;

    private Action _currentSate;

    private bool _hasGameStarted = false;

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
        DontDestroyOnLoad(gameObject);
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
    public void QuitLevel()
    {
        InitNotInLevel();
    }

    public void StartGame()
    {
        InitGamePlaying();
    }

    public void RestartGame()
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
        InitGamePlaying();
    }

    private void InitGamePlaying()
    {
        TempoManager.Instance.ResetTime();
        _currentGameState = GameState.InGame;
        _currentSate = GamePlaying;
        
        if (_hasGameStarted)
        {
            onGameRestart?.Invoke();
        }
        else
        {
            _hasGameStarted = true;
            onGameStart?.Invoke();
        }            
    }

    private void GamePlaying()
    {

    }

    private void InitGameEnding()
    {
        _currentGameState = GameState.EndingLevel;
        _currentSate = GameEnding;
        _hasGameStarted = false;

        int currentLevel = PlayerData.GetCurrentLevel();
        PlayerData.UnlockLevel(currentLevel + 1);

        if (LevelEndPanel.Instance != null)
            LevelEndPanel.Instance.ShowPanel(currentLevel);
    }

    private void GameEnding()
    {

    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}