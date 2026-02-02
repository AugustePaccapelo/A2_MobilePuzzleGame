using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections.Generic;
using System.Linq;

// Author : Auguste Paccapelo

public class InputManager : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Singleton ----- \\

    public static InputManager Instance {get; private set;}

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private List<FingerInput> _allFingers = new();

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += NewFingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= NewFingerDown;
    }

    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Debug.Log(nameof(InputManager) + " Instance already exist, destorying last added.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start() { }

    void Update() { }

    private void LateUpdate()
    {
        int length = _allFingers.Count;
        FingerInput finger;
        for (int i = length - 1; i > -1; i--)
        {
            finger = _allFingers[i];
            if (finger.finger.currentTouch.ended)
            {
                _allFingers.RemoveAt(i);
            }
        }
    }

    // ----- My Functions ----- \\

    private void NewFingerDown(Finger obj)
    {
        FingerInput newFinger = new FingerInput(obj);

        _allFingers.Add(newFinger);
    }

    public FingerInput GetNewFingerAtPos(Vector2 pos, bool includeTrackedFingers = false)
    {
        foreach (FingerInput finger in  _allFingers)
        {
            if (finger.finger.screenPosition == pos && (includeTrackedFingers || !finger.isTracked))
            {
                return finger;
            }
        }

        return null;
    }

    public FingerInput GetNewFingerAndDontTrack(bool includeTrackedFingers = false)
    {
        return _allFingers.First(f => includeTrackedFingers || !f.isTracked);
    }

    public FingerInput GetNewFingerAndTrack()
    {
        FingerInput finger = GetNewFingerAndDontTrack();
        finger.isTracked = true;
        return finger;
    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}