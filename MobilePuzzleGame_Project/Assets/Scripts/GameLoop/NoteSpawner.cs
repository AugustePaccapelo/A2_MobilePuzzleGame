using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Author : Auguste Paccapelo

public class NoteSpawner : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    [SerializeField] private List<Sprite> _sprites = new();

    // ----- Objects ----- \\

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private Transform _noteContainer;
    private TempoDecoder _tempoDecoder;

    private Ball _currentNote = null;

    // ----- Events ----- \\
    public event Action OnNoteSpawn;

    // ----- Others ----- \\

    [SerializeField] private Vector2 _initialVelocity;

    [SerializeField] private int _id = 1;
    public int Id
    {
        get => _id;
        set
        {
            if (value < 1)
            {
                Debug.LogWarning(name + ": id cannot be less than 1.");
                _id = 1;
                return;
            }
            _id = value;
        }
    }

    private bool _canSpawnNote = false;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        GameManager.onGameStart += OnGameStart;
        _tempoDecoder.OnBeat += OnBeat;
        GameObject noteGo = Instantiate(_notePrefab, _noteContainer);
        noteGo.SetActive(false);
        _currentNote = noteGo.GetComponent<Ball>();
        SpawnNote();

        ObstaclesPlacer.onObstacleSelected += NewObstacleSelected;
        ObstaclesPlacer.onObstacleUnselected += ObstacleUnselected;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
        _tempoDecoder.OnBeat -= OnBeat;
        if (_currentNote != null) Destroy(_currentNote);

        ObstaclesPlacer.onObstacleSelected -= NewObstacleSelected;
        ObstaclesPlacer.onObstacleUnselected -= ObstacleUnselected;
    }

    private void OnValidate()
    {
        Id = _id;
        #if UNITY_EDITOR
            EditorApplication.delayCall += DelayFuncToShutUpUnity;
        #endif
    }

    private void Awake()
    {
        _tempoDecoder = GetComponent<TempoDecoder>();
        Id = _id;
    }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void ObstacleUnselected()
    {
        _canSpawnNote = true;
        TempoManager.Instance.ResetTime();
        GameManager.Instance.RestartGame();
    }

    private void NewObstacleSelected()
    {
        _canSpawnNote = false;
        if (_currentNote == null) return;
        _currentNote.gameObject.SetActive(false);
    }

    private void DelayFuncToShutUpUnity()
    {
        #if UNITY_EDITOR
            EditorApplication.delayCall -= DelayFuncToShutUpUnity;
        #endif

        if (this == null || gameObject == null) return;

        
    }

    private void OnBeat()
    {
        if (_canSpawnNote)
        {
            SpawnNote();
            _canSpawnNote = false;
            return;
        }
    }

    private void OnGameStart()
    {
        //_canSpawnNote = true;
    }

    private void SpawnNote()
    {
        GameObject newNote = _currentNote.gameObject;
        newNote.transform.position = _spawnPos.position;
        newNote.SetActive(true);
        newNote.GetComponent<Rigidbody2D>().linearVelocity = _initialVelocity;
        _currentNote.Id = _id;
        Ball.TriggerOnBallRespawn();
        OnNoteSpawn?.Invoke();
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}