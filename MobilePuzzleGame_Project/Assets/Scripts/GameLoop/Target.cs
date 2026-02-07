using UnityEngine;

// Author : Auguste Paccapelo

public class Target : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}