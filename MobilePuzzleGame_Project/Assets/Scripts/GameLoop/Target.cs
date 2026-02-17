using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Author : Auguste Paccapelo

public class Target : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    [SerializeField] private List<Sprite> _sprites = new();

    // ----- Objects ----- \\

    [SerializeField] private SpriteRenderer _renderer;

    // ----- Others ----- \\

    [SerializeField] private int _id = 1;
    public int Id
    {
        get => _id;
        set
        {
            if (value < 1)
            {
                Debug.LogWarning(name + ": id cannot be less than 1.");
                _id = 1;
                return;
            }
            _id = value;
        }
    }
    [SerializeField] private LayerMask _layerToDestroy;

    private static int _numTargets = 0;
    private static int _numTargetsFinished = 0;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void OnValidate()
    {
        Id = _id;
        #if UNITY_EDITOR
            EditorApplication.delayCall += DelayFuncToShutUpUnity;
        #endif
    }

    private void Awake()
    {
        Id = _id;
        SetSprite();

        _numTargets++;
        _numTargetsFinished = 0;
    }

    private void Start() { }

    private void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1 << go.layer => exact layerMask of go
        // if Note = 6 => 00100000
        if (((1 << collision.gameObject.layer) & _layerToDestroy) == 0) return;

        Destroy(collision.gameObject);

        GhostNote ghostComp;
        if (collision.TryGetComponent(out ghostComp))
        {
            return;
        }

        Ball note = collision.GetComponent<Ball>();

        if (note.Id == _id)
        {
            _numTargetsFinished++;
            if (_numTargetsFinished == _numTargets)
            {
                GameManager.Instance.FinishLevel();
            }
        }
        else
        {
            GameManager.Instance.RestartGame();
            _numTargetsFinished = 0;
        }
    }

    // ----- My Functions ----- \\

    private void DelayFuncToShutUpUnity()
    {
        #if UNITY_EDITOR
            EditorApplication.delayCall -= DelayFuncToShutUpUnity;
        #endif
        if (this == null || gameObject == null) return;

        SetSprite();
    }

    private void SetSprite()
    {
        if (_sprites.Count < 0)
        {
            Debug.LogWarning(name + ": no sprites were given.");
            return;
        }

        if (_id <= _sprites.Count)
        {
            _renderer.sprite = _sprites[_id - 1];
        }
        else
        {
            Debug.LogError(name + ": key id not in the sprites.");
            _renderer.sprite = _sprites[0];
        }
    }

    // ----- Destructor ----- \\

    private void OnDestroy()
    {
        _numTargets--;
        if (_numTargets < 0) _numTargets = 0;
        _numTargetsFinished = 0;
    }
}