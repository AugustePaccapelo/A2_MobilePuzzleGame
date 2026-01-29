using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

// Author : Auguste Paccapelo

public class Note : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    static private Dictionary<int, List<Note>> _mapActivesNotes = new();

    private SpriteRenderer _renderer;

    // ----- Others ----- \\

    [SerializeField] private int _level = 0;
    public int Level => _level;

    [SerializeField] private float _triggerWaveRadius = 2.0f;
    public float TriggerWaveRadius => _triggerWaveRadius;

    [SerializeField] private LayerMask _triggerWaveLayerMask = 0;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void Reset()
    {
        _level = 0;
        _triggerWaveRadius = 2.0f;
        _triggerWaveLayerMask = 0;

        _renderer = GetComponentInChildren<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogWarning(name + ": no sprite renderer found.");
        }
    }

    private void OnValidate()
    {
        if (_level < 0)
        {
            _level = 0;
            Debug.LogWarning(name + ": level of a note cannot be negative");
        }

        if (_triggerWaveRadius < 0)
        {
            _triggerWaveRadius = 0f;
            Debug.LogWarning(name + ": trigger wave radius cannot be negative");
        }
    }

    private void OnEnable()
    {
        AddMyselfToMap();
    }

    private void OnDisable()
    {
        RemoveMyselfFromMap();
    }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void TriggerWave()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _triggerWaveRadius, Vector2.zero, 0, _triggerWaveLayerMask);
        IInteractible obj;
        foreach (RaycastHit2D hit in hits)
        {
            obj = hit.transform.GetComponent<IInteractible>();
            if (obj == null) continue;
            Debug.Log("Triggering an action");
            obj.TriggerAction();
        }
    }

    static public void TriggerNote(int levelToTrigger)
    {
        // If no notes of given level exist
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
            // Create the list
            _mapActivesNotes[_level] = new();
        }
        // Add itself to list
        _mapActivesNotes[_level].Add(this);
    }

    private void RemoveMyselfFromMap()
    {
        // If the key doesn't exist
        if (!_mapActivesNotes.ContainsKey(_level)) return;
        // If no list exist, cannot be in
        if (_mapActivesNotes[_level] == null) return;
        // Is not in the list
        if (!_mapActivesNotes[_level].Contains(this)) return;
        // Remove itself from list
        _mapActivesNotes[_level].Remove(this);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}