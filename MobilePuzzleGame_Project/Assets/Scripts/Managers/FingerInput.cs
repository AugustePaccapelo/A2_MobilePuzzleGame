using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

// Author : Auguste Paccapelo

public class FingerInput
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private Finger _finger;
    public Finger finger => _finger;

    // ----- Others ----- \\

    private bool _isTracked = false;
    public bool isTracked
    {
        get => _isTracked;
        set => _isTracked = value;
    }

    // ---------- FUNCTIONS ---------- \\

    public FingerInput(Finger finger)
    {
        _finger = finger;
    }

    // ----- My Functions ----- \\
}