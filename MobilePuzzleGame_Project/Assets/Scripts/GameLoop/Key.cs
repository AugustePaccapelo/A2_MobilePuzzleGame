using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Author : Auguste Paccapelo

public class Key : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    [SerializeField] private List<Sprite> _sprites = new();

    // ----- Objects ----- \\

    // ----- Events ----- \\

    static public event Action<int> onKeyPickedUp;
    static public event Action<int> onAllKeysPickedUp;

    // ----- Others ----- \\

    [SerializeField] private int _keyId = 1;
    public int KeyId
    {
        get => _keyId;
        set
        {
            if (value < 1)
            {
                Debug.Log(name + ": id cannot be less than 1.");
                _keyId = 1;
                return;
            }
            _keyId = value;
        }
    }

    [SerializeField] private LayerMask _layerThatCanCollect;

    // Id; number
    static private Dictionary<int, int> _mapNumKeys = new();
    static private Dictionary<int, int> _mapNumKeysPickedUp = new();

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        GameManager.onGameRestart += OnRestart;
    }

    private void OnDisable()
    {
        GameManager.onGameRestart -= OnRestart;
    }

    private void Awake()
    {
        _mapNumKeys.Clear();
        _mapNumKeysPickedUp.Clear();

        SetSprite();
    }

    private void OnValidate()
    {
        KeyId = _keyId;
        EditorApplication.delayCall += DelayFuncToShutUpUnity;
    }

    private void Start()
    {
        if (!_mapNumKeys.ContainsKey(_keyId))
        {
            _mapNumKeys.Add(_keyId, 1);            
        }
        else _mapNumKeys[_keyId]++;

        if (!_mapNumKeysPickedUp.ContainsKey(_keyId))
        {
            _mapNumKeysPickedUp.Add(_keyId, 0);
        }
    }

    private void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1 << go.layer => exact layerMask of go
        // if Note = 6 => 00100000
        if (((1 << collision.gameObject.layer) & _layerThatCanCollect) == 0) return;

        _mapNumKeysPickedUp[_keyId]++;
        onKeyPickedUp?.Invoke(_keyId);

        if (_mapNumKeysPickedUp[_keyId] == _mapNumKeys[_keyId])
        {
            onAllKeysPickedUp?.Invoke(_keyId);
        }

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    // ----- My Functions ----- \\

    private void OnRestart()
    {
        gameObject.SetActive(true);
        _mapNumKeysPickedUp[_keyId] = 0;
    }

    private void DelayFuncToShutUpUnity()
    {
        EditorApplication.delayCall -= DelayFuncToShutUpUnity;

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

        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();

        if (_keyId <= _sprites.Count)
        {
            renderer.sprite = _sprites[_keyId - 1];
        }
        else
        {
            Debug.LogError(name + ": key id not in the sprites.");
            renderer.sprite = _sprites[0];
        }
    }

    // ----- Destructor ----- \\

    private void OnDestroy() {}
}