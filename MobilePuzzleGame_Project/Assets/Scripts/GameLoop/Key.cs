using System;
using UnityEngine;
using UnityEngine.Rendering;

// Author : Auguste Paccapelo

public class Key : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\

    static public event Action onKeyPickedUp;
    static public event Action onAllKeysPickedUp;

    // ----- Others ----- \\

    static private int _numKeys = 0;
    static private int _numKeysPickedUp = 0;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start()
    {
        _numKeys++;
    }

    private void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _numKeysPickedUp++;
        onKeyPickedUp?.Invoke();
        if (_numKeysPickedUp == _numKeys)
        {
            onAllKeysPickedUp?.Invoke();
        }

        Destroy(gameObject);
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}