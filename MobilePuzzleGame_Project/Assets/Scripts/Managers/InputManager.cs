using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.EventSystems;

// Author : Auguste Paccapelo

public struct ToucheData
{
    public Vector2 screenPosition;
    public Vector3 worldPosition;

    public FingerInput fingerInput;
}

public class InputManager : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Singleton ----- \\

    public static InputManager Instance {get; private set;}

    // ----- Objects ----- \\

    private List<FingerInput> _allFingers = new();

    // ----- Events ----- \\

    public event Action<Vector2> onFingerDown;
    public event Action<Vector2> onFingerMove;
    public event Action<Vector2> onFingerUp;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += NewFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= NewFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;
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
        List<RaycastResult> uiRaycastResults = UIRaycast(obj.screenPosition);

        if (CallUITouchedDown(uiRaycastResults)) return;

        RaycastHit2D physicRaycastResult = PhysiscRaycast(obj.screenPosition);
        if (CallPhysicsTouchedDown(physicRaycastResult, obj.screenPosition)) return;

        onFingerDown?.Invoke(obj.screenPosition);
    }

    private void OnFingerMove(Finger obj)
    {
        onFingerMove?.Invoke(obj.screenPosition);
    }

    private void OnFingerUp(Finger obj)
    {
        List<RaycastResult> uiRaycastResults = UIRaycast(obj.screenPosition);

        if (CallUITouchedUp(uiRaycastResults)) return;

        RaycastHit2D physicRaycastResult = PhysiscRaycast(obj.screenPosition);
        if (CallPhysicsTouchedUp(physicRaycastResult, obj.screenPosition)) return;

        onFingerUp?.Invoke(obj.screenPosition);
    }

    // Raycasts

    private RaycastHit2D PhysiscRaycast(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        return hit;
    }

    private List<RaycastResult> UIRaycast(Vector2 screenPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results;
    }

    // Touched Down calls

    private bool CallPhysicsTouchedDown(RaycastHit2D hit, Vector2 screenPos)
    {
        if (hit.collider == null) return false;

        ITouchableOnDown touchable;
        if (!hit.collider.TryGetComponent(out touchable)) return false;

        ToucheData newData = new();
        newData.screenPosition = screenPos;
        newData.worldPosition = hit.point;
        newData.fingerInput = GetNewFingerAtPosAndDontTrack(screenPos);

        touchable.OnTouchedDown(newData);

        return true;
    }

    private bool CallUITouchedDown(List<RaycastResult> results)
    {
        ITouchableOnDown touchable;

        bool hasTouchSomething = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out touchable))
            {
                ToucheData newData = new();
                newData.screenPosition = result.screenPosition;
                newData.worldPosition = result.worldPosition;
                newData.fingerInput = GetNewFingerAtPosAndDontTrack(result.screenPosition);

                touchable.OnTouchedDown(newData);

                hasTouchSomething = true;
            }
        }

        return hasTouchSomething;
    }

    // Touched Up Calls

    private bool CallPhysicsTouchedUp(RaycastHit2D hit, Vector2 screenPos)
    {
        if (hit.collider == null) return false;

        ITouchableOnUp touchable;
        if (!hit.collider.TryGetComponent(out touchable)) return false;

        ToucheData newData = new();
        newData.screenPosition = screenPos;
        newData.worldPosition = hit.point;
        newData.fingerInput = GetNewFingerAtPosAndDontTrack(screenPos);

        touchable.OnTouchedUp(newData);

        return true;
    }

    private bool CallUITouchedUp(List<RaycastResult> results)
    {
        ITouchableOnUp touchable;

        bool hasTouchSomething = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out touchable))
            {
                ToucheData newData = new();
                newData.screenPosition = result.screenPosition;
                newData.worldPosition = result.worldPosition;
                newData.fingerInput = GetNewFingerAtPosAndDontTrack(result.screenPosition);

                touchable.OnTouchedUp(newData);

                hasTouchSomething = true;
            }
        }

        return hasTouchSomething;
    }

    // Fingers getters

    public FingerInput GetNewFingerAtPosAndTrack(Vector2 pos, bool includeTrackedFingers = false)
    {
        foreach (FingerInput finger in  _allFingers)
        {
            if (finger.finger.screenPosition == pos && (includeTrackedFingers || !finger.isTracked))
            {
                finger.isTracked = true;
                return finger;
            }
        }

        return null;
    }

    public FingerInput GetNewFingerAtPosAndDontTrack(Vector2 pos, bool includeTrackedFingers = false)
    {
        foreach (FingerInput finger in _allFingers)
        {
            if (finger.finger.screenPosition == pos && (includeTrackedFingers || !finger.isTracked))
            {
                finger.isTracked = false;
                return finger;
            }
        }

        return null;
    }

    public FingerInput GetNewFingerAndDontTrack(bool includeTrackedFingers = false)
    {
        return _allFingers.First(f => includeTrackedFingers || !f.isTracked);
    }

    public FingerInput GetNewFingerAndTrack(bool includeTrackedFingers = false)
    {
        FingerInput finger = GetNewFingerAndDontTrack(includeTrackedFingers);
        finger.isTracked = true;
        return finger;
    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}