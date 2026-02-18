using UnityEngine;

public class MusicCoordinator : MonoBehaviour
{
    public static MusicCoordinator Instance { get; private set; }

    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource levelMusicSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (menuMusicSource == null)
        {
            menuMusicSource = gameObject.AddComponent<AudioSource>();
        }
        if (levelMusicSource == null)
        {
            levelMusicSource = gameObject.AddComponent<AudioSource>();
        }

        menuMusicSource.clip = menuMusic;
        menuMusicSource.loop = true;
        menuMusicSource.playOnAwake = true;

        levelMusicSource.clip = levelMusic;
        levelMusicSource.loop = true;
        levelMusicSource.playOnAwake = false;

        if (!menuMusicSource.isPlaying && menuMusic != null)
            menuMusicSource.Play();
        
        if (!levelMusicSource.isPlaying && levelMusic != null)
            levelMusicSource.Play();

        GameManager.onGameStart += OnGameStart;
        GameManager.onGameRestart += OnGameRestart;
    }

    void Update()
    {
        UpdateMusicState();
    }

    private void UpdateMusicState()
    {
        if (GameManager.CurrentGameState == GameState.NotInLevel)
        {
            if (menuMusicSource != null)
            {
                menuMusicSource.mute = false;
                if (!menuMusicSource.isPlaying && menuMusic != null)
                    menuMusicSource.Play();
            }
            if (levelMusicSource != null)
            {
                levelMusicSource.mute = true;
            }
        }
        else
        {
            if (menuMusicSource != null)
            {
                menuMusicSource.mute = true;
            }
            if (levelMusicSource != null)
            {
                levelMusicSource.mute = false;
                if (!levelMusicSource.isPlaying && levelMusic != null)
                    levelMusicSource.Play();
            }
        }
    }

    private void OnGameStart()
    {
        if (levelMusicSource != null && !levelMusicSource.isPlaying && levelMusic != null)
        {
            levelMusicSource.Play();
        }
    }

    private void OnGameRestart()
    {
        if (levelMusicSource != null && !levelMusicSource.isPlaying && levelMusic != null)
        {
            levelMusicSource.Play();
        }
    }

    private void OnDestroy()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameRestart -= OnGameRestart;

        if (Instance == this) Instance = null;
    }
}
