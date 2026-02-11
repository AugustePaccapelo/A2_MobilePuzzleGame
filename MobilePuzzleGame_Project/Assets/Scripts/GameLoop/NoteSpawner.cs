using UnityEngine;

// Author : Auguste Paccapelo

public enum NoteColor
{
    Black, Red, Blue, Green
}

public class NoteSpawner : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private Transform _spawnPos;
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private GameObject _ghostNotePrefab;
    [SerializeField] private Transform _noteContainer;
    private TempoDecoder _tempoDecoder;

    // ----- Others ----- \\

    [SerializeField] private Vector2 _initialVelocity;

    private bool _hasGameStarted = false;
    private bool _hasSpawnedNote = false;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        GameManager.onGameStart += OnGameStart;
        _tempoDecoder.OnBeat += OnBeat;
    }    

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
        _tempoDecoder.OnBeat -= OnBeat;
    }

    private void Awake()
    {
        _tempoDecoder = GetComponent<TempoDecoder>();
    }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnBeat()
    {
        if (_hasGameStarted)
        {
            if (_hasSpawnedNote) return;
            SpawnNote();
            _hasSpawnedNote = true;
            return;
        }

        SpawnGhostNote();
    }

    private void OnGameStart()
    {
        _hasGameStarted = true;
        _hasSpawnedNote = false;
    }

    private void SpawnNote()
    {
        GameObject newNote = Instantiate(_notePrefab, _noteContainer);
        newNote.transform.position = _spawnPos.position;
        newNote.GetComponent<Rigidbody2D>().linearVelocity = _initialVelocity;
    }

    private void SpawnGhostNote()
    {
        GameObject newNote = Instantiate(_ghostNotePrefab, _noteContainer);
        newNote.transform.position = _spawnPos.position;
        newNote.GetComponent<Rigidbody2D>().linearVelocity = _initialVelocity;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}