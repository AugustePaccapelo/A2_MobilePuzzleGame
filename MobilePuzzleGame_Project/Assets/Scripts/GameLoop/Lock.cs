using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

// Author : Auguste Paccapelo

public class Lock : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    [SerializeField] private List<Sprite> _sprites = new();

    // ----- Objects ----- \\

    [SerializeField] private SpriteRenderer _lockRenderer;

    // ----- Others ----- \\

    [SerializeField] private int _lockId = 1;
    public int LockId
    {
        get => _lockId;
        set
        {
            if (value < 1)
            {
                Debug.Log(name + ": id cannot be less than 1.");
                _lockId = 1;
                return;
            }
            _lockId = value;
        }
    }

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        Key.onAllKeysPickedUp += DestroyLock;
        GameManager.onGameRestart += OnRestart;
    }

    private void OnDisable()
    {
        Key.onAllKeysPickedUp -= DestroyLock;
        GameManager.onGameRestart -= OnRestart;
    }

    private void Awake()
    {
        SetSprite();
    }

    private void OnValidate()
    {
        LockId = _lockId;

        #if UNITY_EDITOR
            EditorApplication.delayCall += DelayFuncToShutUpUnity;
        #endif
    }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnRestart()
    {
        gameObject.SetActive(true);
    }

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

        if (_lockId <= _sprites.Count)
        {
            _lockRenderer.sprite = _sprites[_lockId - 1];
        }
        else
        {
            Debug.LogError(name + ": key id not in the sprites.");
            _lockRenderer.sprite = _sprites[0];
        }
    }

    private void DestroyLock(int id)
    {
        if (id != _lockId) return;

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}