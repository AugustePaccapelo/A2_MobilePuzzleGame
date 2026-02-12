using UnityEngine;

// Author : Auguste Paccapelo

public class GhostNote : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    [SerializeField] private int _numBeatToSurvive = 0;
    private int _currentNumBeat = 0;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        TempoManager.Instance.OnBeat += OnBeat;
        GameManager.onGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        TempoManager.Instance.OnBeat -= OnBeat;
        GameManager.onGameStart -= OnGameStart;
    }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnGameStart()
    {
        Destroy(gameObject);
    }

    private void OnBeat(int obj)
    {
        _currentNumBeat++;
        if (_currentNumBeat >= _numBeatToSurvive)
        {
            Destroy(gameObject);
        }
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}