using System.Collections.Generic;
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

    // ----- Others ----- \\

    [SerializeField] private float _ghostNoteSpawnDelay = 2f;
    private float _timerGhostNote = 0f;

    [SerializeField] private Vector2 _initialVelocity;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        GameManager.onGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= OnGameStart;
    }

    private void Awake() { }

    private void Start() { }

    private void Update()
    {
        if (GameManager.CurrentGameState != GameState.PlayerPlacingPlatforms) return;

        _timerGhostNote += Time.deltaTime;
        if (_timerGhostNote >= _ghostNoteSpawnDelay)
        {
            _timerGhostNote = 0f;
            SpawnGhostNote();
        }
    }

    // ----- My Functions ----- \\

    private void OnGameStart()
    {
        SpawnNote();
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