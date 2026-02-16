using UnityEngine;

// Author : Auguste Paccapelo

public class ObstacleFeedBack : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private GameObject _lightEffectGo;
    private TempoDecoder _tempoDecoder;

    // ----- Others ----- \\

    [SerializeField] private float _rotatingSpeed = 360;

    private bool _isRotating;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        if (_tempoDecoder == null) return;

        _tempoDecoder.OnBeat += OnBeat;
        _tempoDecoder.OnBeatAfter += OnBeatAfter;
    }

    private void OnDisable()
    {
        if (_tempoDecoder == null) return;

        _tempoDecoder.OnBeat -= OnBeat;
        _tempoDecoder.OnBeatAfter -= OnBeatAfter;
    }

    private void Awake()
    {
        _tempoDecoder = GetComponentInChildren<TempoDecoder>();

        if (_tempoDecoder == null) Destroy(this);
    }

    private void Start() { }

    private void Update()
    {
        if (!_isRotating) return;

        Vector3 angle = _lightEffectGo.transform.eulerAngles;
        angle.z += _rotatingSpeed * Time.deltaTime;
        _lightEffectGo.transform.eulerAngles = angle;
    }

    // ----- My Functions ----- \\

    private void OnBeatAfter()
    {
        _lightEffectGo.SetActive(false);
        _isRotating = false;
        _lightEffectGo.transform.eulerAngles = Vector3.zero;
    }

    private void OnBeat()
    {
        _lightEffectGo.SetActive(true);
        _isRotating = true;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}