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

    private bool _hasGameStarted = false;
    private bool _canSpawnNote = false;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        GameManager.onGameStart += OnGameStart;
        _tempoDecoder.OnBeat += OnBeat;
        GameObject noteGo = Instantiate(_notePrefab, _noteContainer);
        _currentNote = noteGo.GetComponent<Ball>();

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
        EditorApplication.delayCall += DelayFuncToShutUpUnity;
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
    }

    private void NewObstacleSelected()
    {
        _canSpawnNote = false;
        _currentNote.gameObject.SetActive(false);
    }

    private void DelayFuncToShutUpUnity()
    {
        EditorApplication.delayCall -= DelayFuncToShutUpUnity;

        if (this == null || gameObject == null) return;

        SetSprite();
    }

    private void SetSprite()
    {
        if (_sprites.Count < 0)
        {
            Debug.LogWarning(name + ": no sprites were given.");
            return;
        }

        if (_id <= _sprites.Count)
        {
            _renderer.sprite = _sprites[_id - 1];
        }
        else
        {
            Debug.LogError(name + ": key id not in the sprites.");
            _renderer.sprite = _sprites[0];
        }
    }

    private void OnBeat()
    {
        if (_hasGameStarted && _canSpawnNote)
        {
            SpawnNote();
            _canSpawnNote = false;
            return;
        }
    }

    private void OnGameStart()
    {
        _hasGameStarted = true;
        _canSpawnNote = true;
    }

    private void SpawnNote()
    {
        GameObject newNote = _currentNote.gameObject;
        newNote.SetActive(true);
        newNote.transform.position = _spawnPos.position;
        newNote.GetComponent<Rigidbody2D>().linearVelocity = _initialVelocity;
        _currentNote.Id = _id;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}