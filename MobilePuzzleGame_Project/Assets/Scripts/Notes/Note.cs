using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Author : Auguste Paccapelo

public class Note : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    static private Dictionary<int, List<Note>> _mapActivesNotes = new();

    private SpriteRenderer _renderer;
    private Rigidbody2D _rigidBody;

    // ----- Events ----- \\
    [Header("Note")]
    [SerializeField] private UnityEvent _onTriggerWave;

    // ----- Others ----- \\

    static private int _numberActiveNotes = 0;
    static public int NumberActiveNotes => _numberActiveNotes;

    [SerializeField] private int _level = 0;
    public int Level
    {
        get => _level;
        private set
        {
            if (value < 0)
            {
                _level = 0;
                Debug.LogWarning(name + ": level of a note cannot be negative.");
                return;
            }
            _level = value;
        }
    }

    [SerializeField] private float _triggerWaveRadius = 2.0f;
    public float TriggerWaveRadius
    {
        get => _triggerWaveRadius;
        private set
        {
            if (value < 0)
            {
                _triggerWaveRadius = 0f;
                Debug.LogWarning(name + ": trigger wave radius cannot be negative.");
                return;
            }
            _triggerWaveRadius = value;
        }
    }

    [SerializeField] private LayerMask _triggerWaveLayerMask = 0;

    [Header("Physics")]
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _gravityScale = 1.0f;

    [SerializeField] private Vector2 _gravityDirection = Vector2.down;

    [SerializeField] private float _mass = 1.0f;
    public float Mass
    {
        get => _mass;
        private set
        {
            if (value < 0)
            {
                _mass = 0f;
                Debug.LogWarning(name + ": mass cannot be negative.");
                return;
            }
            _mass = value;
        }
    }

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void Reset()
    {
        // Unity
        GetRigidBody();
        GetRenderer();

        // Notes
        _level = 0;
        _triggerWaveRadius = 2.0f;
        _triggerWaveLayerMask = 0;

        // Physics
        _gravity = 9.81f;
        _gravityScale = 1.0f;
        _gravityDirection = Vector2.down;
        _mass = 1.0f;
    }

    private void OnValidate()
    {
        Level = _level;
        TriggerWaveRadius = _triggerWaveRadius;
        Mass = _mass;
    }

    private void OnEnable()
    {
        AddMyselfToMap();
    }

    private void OnDisable()
    {
        RemoveMyselfFromMap();
    }

    private void Awake()
    {
        if (_rigidBody == null) GetRigidBody();
        if (_renderer == null) GetRenderer();
    }

    private void Start() { }

    private void Update()
    {
        if (GameManager.CurrentGameState != GameState.GamePlaying) return;

        ApplyGravity();
    }

    // ----- My Functions ----- \\

    private void GetRigidBody()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogWarning(name + ": no rigidBody found.");
        }
    }

    private void GetRenderer()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogWarning(name + ": no sprite renderer found.");
        }
    }

    private void TriggerWave()
    {
        // Circle cast around the note
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _triggerWaveRadius, Vector2.zero, 0, _triggerWaveLayerMask);
        IInteractible obj;
        
        foreach (RaycastHit2D hit in hits)
        {
            obj = hit.transform.GetComponentInChildren<IInteractible>();
            // If not an IInteractible continue
            if (obj == null) continue;
            
            obj.TriggerAction();
        }
    }

    static public void TriggerNote(int levelToTrigger)
    {
        // If no notes of given level exist return
        if (!_mapActivesNotes.ContainsKey(levelToTrigger) || _mapActivesNotes[levelToTrigger] == null) return;

        foreach (Note note in _mapActivesNotes[levelToTrigger])
        {
            note.TriggerWave();
        }
    }

    private void AddMyselfToMap()
    {
        // If no notes of this level have been added
        if (!_mapActivesNotes.ContainsKey(_level) || _mapActivesNotes[_level] == null)
        {
            _mapActivesNotes[_level] = new();
        }
        
        _mapActivesNotes[_level].Add(this);
        _numberActiveNotes++;
    }

    private void RemoveMyselfFromMap()
    {
        // If the key doesn't exist return
        if (!_mapActivesNotes.ContainsKey(_level) || _mapActivesNotes[_level] == null) return;
        // If is not in the list
        if (!_mapActivesNotes[_level].Contains(this)) return;

        _mapActivesNotes[_level].Remove(this);
        _numberActiveNotes--;
    }

    private void ApplyGravity()
    {
        Vector2 gravityAcceleration = _gravityDirection * _gravity * _gravityScale;
        ApplyAcceleration(gravityAcceleration);
    }

    public void ApplyAcceleration(Vector2 acceleration)
    {
        _rigidBody.linearVelocity += acceleration * Time.deltaTime;
    }

    public void ApplyForce(Vector2 force)
    {
        Vector2 acceleration = force / _mass;
        ApplyAcceleration(acceleration);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}