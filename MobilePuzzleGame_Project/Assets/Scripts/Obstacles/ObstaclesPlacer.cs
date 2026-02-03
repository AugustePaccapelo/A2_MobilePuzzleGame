using UnityEngine;

// Author : Auguste Paccapelo

public class ObstaclesPlacer : MonoBehaviour, ITouchableOnDown, ITouchableOnUp
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private FingerInput _currentFinger = null;
    private UIObstacleSpawner _creator;
    public UIObstacleSpawner Creator
    {
        get => _creator;
        set => _creator = value;
    }

    // ----- Others ----- \\

    static private bool _hasAnObstacleSelected = false;
    static private ObstaclesPlacer _currentObstacleSelected = null;

    private bool _isThisSelected = false;
    private bool _wasSelectedThisFrame = false;
    private bool _wasOnFingerDown = false;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update()
    {
        // If no finger return
        if (_currentFinger == null || !_currentFinger.finger.isActive)
        {
            return;
        }

        // If finger is moving
        if (_currentFinger.finger.currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(_currentFinger.finger.screenPosition);
            pos.z = transform.position.z;
            transform.position = pos;

            if (_wasSelectedThisFrame)
            {
                _wasSelectedThisFrame = false;
            }
        }
    }

    public void OnTouchedDown(Vector2 screenPos)
    {
        _wasOnFingerDown = true;
        _currentFinger = InputManager.Instance.GetNewFingerAtPos(screenPos);
    }

    public void OnTouchedUp(Vector2 screenPos)
    {
        if (!_wasOnFingerDown)
        {
            _currentFinger = null;
            return;
        }

        _wasOnFingerDown = false;

        // If not selected, select
        if (!_isThisSelected)
        {
            Select();
        }
        else
        {
            // If input is tap
            if (_currentFinger.finger.currentTouch.isTap)
            {
                // If was already selected, then turn obstacle
                if (_wasSelectedThisFrame)
                {
                    Vector3 angle = transform.eulerAngles;
                    angle.z += 30;
                    transform.eulerAngles = angle;
                }
                else
                {
                    _wasSelectedThisFrame = false;
                }
            }

            _currentFinger = null;
        }
    }

    // ----- My Functions ----- \\

    static public void UnSelectCurrent()
    {
        if (_currentObstacleSelected != null)
        {
            _currentObstacleSelected._isThisSelected = false;
        }
        
        _hasAnObstacleSelected = false;
        _currentObstacleSelected = null;
    }

    public void Select(FingerInput finger = null)
    {
        if (_hasAnObstacleSelected)
        {
            UnSelectCurrent();
        }

        _wasOnFingerDown = true;
        _isThisSelected = true;
        _hasAnObstacleSelected = true;
        _currentObstacleSelected = this;
        _wasSelectedThisFrame = true;

        if (finger != null)
        {
            _currentFinger = finger;
        }
    }

    static public void PickupCurrentObstacle()
    {
        if (!_hasAnObstacleSelected) return;
        
        _currentObstacleSelected._creator.PickupObstacle(_currentObstacleSelected.gameObject);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}