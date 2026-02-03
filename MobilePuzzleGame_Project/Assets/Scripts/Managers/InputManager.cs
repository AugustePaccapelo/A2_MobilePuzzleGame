using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.EventSystems;

// Author : Auguste Paccapelo

public class InputManager : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Singleton ----- \\

    public static InputManager Instance {get; private set;}

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private List<FingerInput> _allFingers = new();

    // ----- Events ----- \\

    public event Action onFingerDown;

    // ----- Others ----- \\

    [SerializeField] private LayerMask _touchRaycastMask;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += NewFingerDown;
        Touch.onFingerUp += FingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= NewFingerDown;
        Touch.onFingerUp -= FingerUp;
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

        onFingerDown?.Invoke();

        Ray ray = Camera.main.ScreenPointToRay(obj.screenPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Abs(Camera.main.transform.position.z), _touchRaycastMask);
        ITouchableOnDown touchable;

        if (hit.collider != null && hit.collider.TryGetComponent(out touchable))
        {
            touchable.OnTouchedDown(obj.screenPosition);
        }
    }

    private void FingerUp(Finger obj)
    {        
        ITouchableOnUp touchable;

        Ray ray = Camera.main.ScreenPointToRay(obj.screenPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Abs(Camera.main.transform.position.z), _touchRaycastMask);

        if (hit.collider != null && hit.collider.TryGetComponent(out touchable))
        {
            touchable.OnTouchedUp(obj.screenPosition);
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = obj.screenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out touchable))
            {
                touchable.OnTouchedUp(obj.screenPosition);
            }
        }
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