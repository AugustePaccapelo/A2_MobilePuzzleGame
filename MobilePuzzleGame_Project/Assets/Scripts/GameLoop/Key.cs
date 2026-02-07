using System;
using UnityEngine;

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

    [SerializeField] private LayerMask _layerThatCanCollect;

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
        // 1 << go.layer => exact layerMask of go
        // if Note = 6 => 00100000
        if (((1 << collision.gameObject.layer) & _layerThatCanCollect) == 0) return;

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