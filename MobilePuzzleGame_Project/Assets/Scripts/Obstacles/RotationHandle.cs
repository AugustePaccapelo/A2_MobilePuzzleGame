using System;
using UnityEngine;

// Author : Auguste Paccapelo

public class RotationHandle : MonoBehaviour, ITouchableOnDown
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\
    public event Action<ToucheData> onFingerDown;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    public void OnTouchedDown(ToucheData toucheData)
    {
        onFingerDown?.Invoke(toucheData);
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}