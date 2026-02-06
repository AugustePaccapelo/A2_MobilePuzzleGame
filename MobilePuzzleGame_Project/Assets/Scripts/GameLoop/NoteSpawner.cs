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
    [SerializeField] private Transform _noteContainer;

    // ----- Others ----- \\

    [SerializeField] private List<float> _timeOfSpawnOfNotes = new List<float>();
    private float _currentTime = 0f;

    [SerializeField] private Vector2 _initialVelocity;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update()
    {
        if (GameManager.CurrentGameState != GameState.GamePlaying || _timeOfSpawnOfNotes.Count <= 0) return;

        _currentTime += Time.deltaTime;

        int length = _timeOfSpawnOfNotes.Count;
        for (int i = length - 1; i > -1 ; i--)
        {
            if (_currentTime >= _timeOfSpawnOfNotes[i])
            {
                SpawnNote();
                _timeOfSpawnOfNotes.RemoveAt(i);
            }
        }
    }

    // ----- My Functions ----- \\

    private void SpawnNote()
    {
        GameObject newNote = Instantiate(_notePrefab, _noteContainer);
        newNote.transform.position = _spawnPos.position;
        newNote.GetComponent<Rigidbody2D>().linearVelocity = _initialVelocity;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}