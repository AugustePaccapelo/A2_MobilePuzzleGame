using UnityEngine;
using UnityEngine.Events;

// Author : Auguste Paccapelo

public class UIFeedBack : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\

    [SerializeField] private UnityEvent _onBeat;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        TempoManager.Instance.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        TempoManager.Instance.OnBeat -= OnBeat;
    }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnBeat(int obj)
    {
        _onBeat?.Invoke();
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}