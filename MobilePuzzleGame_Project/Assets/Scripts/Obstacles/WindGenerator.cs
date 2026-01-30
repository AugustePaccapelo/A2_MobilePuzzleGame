using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Author : Auguste Paccapelo

public class WindGenerator : MonoBehaviour, IInteractible
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private WindScript _windScript;

    // ----- Events ----- \\
    [SerializeField] private UnityEvent _onActionTriggered;

    // ----- Others ----- \\

    private bool _isActive = true;

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
            }
            _force = value;
        }
    }

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        if (_windScript == null) GetWindScript();
        _windScript.onObjectStay += OnObjectStayInRange;
    }

    private void Start() { }

    private void Update() { }

    private void Reset()
    {
        GetWindScript();
        _force = 5.0f;
    }

    private void OnValidate()
    {
        Force = _force;
    }

    // ----- My Functions ----- \\

    private void OnObjectStayInRange(Collider2D collision)
    {
        if (!_isActive) return;

        Note note = collision.GetComponent<Note>();
        if (note == null) return;

        Debug.Log(transform.up * _force);
        note.ApplyForce(transform.up * _force);
    }

    private void GetWindScript()
    {
        _windScript = GetComponentInChildren<WindScript>();
        if (_windScript == null)
        {
            Debug.LogWarning(name + ": no wind script found.");
        }
    }

    public void TriggerAction()
    {
        _onActionTriggered?.Invoke();
        _isActive = true;
    }

    // ----- Destructor ----- \\

    private void OnDestroy()
    {
        _windScript.onObjectStay -= OnObjectStayInRange;
    }
}