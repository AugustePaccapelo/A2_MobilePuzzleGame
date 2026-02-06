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

    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            _currentGameState = GameState.GamePlaying;
        }
    }

    // ----- My Functions ----- \\

    public void StartGame()
    {
        InitGamePlaying();
    }

    public void LoadLevel()
    {
        InitInitatingLevel();
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

    private void InitPlayerPlacingPlatform()
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
    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}