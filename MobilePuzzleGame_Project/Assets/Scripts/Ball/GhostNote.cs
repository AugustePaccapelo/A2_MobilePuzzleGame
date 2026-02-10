using UnityEngine;

// Author : Auguste Paccapelo

public class GhostNote : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private TempoDecoder _tempoDecoder;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        _tempoDecoder.OnBeat += OnBeat;
    }    

    private void OnDisable()
    {
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
        Destroy(gameObject);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}