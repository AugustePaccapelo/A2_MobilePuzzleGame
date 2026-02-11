using UnityEngine;

// Author : Auguste Paccapelo

public class Lock : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        Key.onAllKeysPickedUp += DestroyLock;
    }

    private void OnDisable()
    {
        Key.onAllKeysPickedUp -= DestroyLock;
    }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void DestroyLock()
    {
        Destroy(gameObject);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}