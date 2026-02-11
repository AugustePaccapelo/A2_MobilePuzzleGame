using UnityEngine;
using UnityEngine.Events;

// Author : Auguste Paccapelo

public class WindGenerator : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private WindScript _windScript;
    private TempoDecoder _tempoDecoder;

    // ----- Events ----- \\

    private UnityEvent _onObjectInWind;
    private UnityEvent _onActivated;
    private UnityEvent _onDeactivated;

    // ----- Others ----- \\

    [SerializeField] private int _numTempoActivated = 1;
    public int NumTempoActivated
    {
        get => _numTempoActivated;
        set
        {
            if (value < 1)
            {
                Debug.LogWarning(name + ": must be activated for at least 1 tempo.");
                _numTempoActivated = 1;
                return;
            }
            _numTempoActivated = value;
        }
    }
    private bool _isActive = false;
    private int _numTempoSinceActivated = 0;


    [SerializeField] private float _force = 5.0f;
    public float Force
    {
        get => _force;
        private set
        {
            if (value < 0)
            {
                Debug.LogWarning(name + ": force cannot be negative.");
                _force = 0;
                return;
            }
            _force = value;
        }
    }

    [SerializeField] private float _forceFallSPeed = 1.0f;
    public float ForceFallSpeed
    {
        get => _forceFallSPeed;
        private set
        {
            if (value < 0)
            {
                Debug.LogWarning(name + ": force fall speed cannot be negative.");
                _forceFallSPeed = 0;
                return;
            }
            _forceFallSPeed = value;
        }
    }

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        _tempoDecoder.OnBeat += OnBeat;
        _tempoDecoder.OnOffBeat += OnOffBeat;
    }

    private void OnDisable() { }

    private void Awake()
    {
        if (_windScript == null) GetWindScript();
        if (_tempoDecoder == null) GetTempoDecoder();
        _windScript.onObjectStay += OnObjectStayInRange;
    }

    private void Start() { }

    private void Update() { }

    private void Reset()
    {
        GetWindScript();
        GetTempoDecoder();
        _force = 5.0f;
        _forceFallSPeed = 1.0f;
        _numTempoActivated = 1;
    }

    private void OnValidate()
    {
        Force = _force;
        ForceFallSpeed = _forceFallSPeed;
        NumTempoActivated = _numTempoActivated;
    }

    // ----- My Functions ----- \\

    private void OnOffBeat()
    {
        if (!_isActive) return;

        _numTempoSinceActivated++;
        if (_numTempoSinceActivated >= _numTempoActivated)
        {            
            _isActive = false;
            _onDeactivated?.Invoke();
        }
    }

    private void OnBeat()
    {
        _isActive = true;
        _onActivated?.Invoke();
        _numTempoSinceActivated = 0;
    }

    private void OnObjectStayInRange(Collider2D collision)
    {
        if (!_isActive) return;
        
        Rigidbody2D noteRB = collision.GetComponent<Rigidbody2D>();

        Vector2 vecToObj = collision.transform.position - transform.position;
        float distance = vecToObj.magnitude;

        float fallOf = distance * _forceFallSPeed;
        float realForce = Mathf.Max(0, _force - fallOf);

        noteRB.AddForce(transform.right * realForce);

        _onObjectInWind?.Invoke();
    }

    private void GetWindScript()
    {
        _windScript = GetComponentInChildren<WindScript>();
        if (_windScript == null)
        {
            Debug.LogWarning(name + ": no wind script found.");
        }
    }

    private void GetTempoDecoder()
    {
        _tempoDecoder = GetComponent<TempoDecoder>();
        if (_tempoDecoder == null)
        {
            Debug.LogWarning(name + ": no tempo decoder found");
        }
    }

    // ----- Destructor ----- \\

    private void OnDestroy()
    {
        _windScript.onObjectStay -= OnObjectStayInRange;
    }
}