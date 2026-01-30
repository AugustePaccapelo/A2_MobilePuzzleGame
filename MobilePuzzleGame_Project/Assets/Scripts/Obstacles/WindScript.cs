using System;
using UnityEngine;

// Author : Auguste Paccapelo

public class WindScript : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\
    public event Action<Collider2D> onObjectStay;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onObjectStay?.Invoke(collision);
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}