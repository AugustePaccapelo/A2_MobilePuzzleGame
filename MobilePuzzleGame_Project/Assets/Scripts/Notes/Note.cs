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

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void Reset()
    {
        // Unity
        GetRenderer();

        // Notes
        _level = 0;
        _triggerWaveRadius = 2.0f;
        _triggerWaveLayerMask = 0;
    }

    private void OnValidate()
    {
        Level = _level;
        TriggerWaveRadius = _triggerWaveRadius;
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
        if (_renderer == null) GetRenderer();
    }

    private void Start() { }

    private void Update()
    {
        if (GameManager.CurrentGameState != GameState.GamePlaying) return;
    }

    // ----- My Functions ----- \\

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

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}