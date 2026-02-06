using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

// Author : Auguste Paccapelo

public class ObstaclesPlacer : MonoBehaviour, ITouchableOnDown, ITouchableOnUp
{
    private enum FingerState
    {
        Moving,
        Rotating
    }

    // ---------- VARIABLES ---------- \\

    // ----- Objects ----- \\

    [SerializeField] private GameObject _mainObject;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _buttonsContainer;

    [SerializeField] private RectTransform _rotationIndicator;

    private Finger _currentFinger;
    public Finger CurrentFinger => _currentFinger;
    static private List<ObstaclesPlacer> _allObstacles = new();

    private RotationHandle _rotationHandle;

    // ----- Events ----- \\

    static public event Action<PlacableObstacle> onObstaclePickedUp;

    // ----- Others ----- \\

    [SerializeField] private PlacableObstacle _obstacleType;

    private bool _isThisSelected = false;
    public bool IsThisSelected => _isThisSelected;

    static private bool _hasAnObstacleSelected = false;
    static public bool HasAnObstacleSelected => _hasAnObstacleSelected;

    static private ObstaclesPlacer _currentObstacleSelected;
    static public ObstaclesPlacer CurrentObstacleSelected => _currentObstacleSelected;
    private FingerState _currentFingerState;

    private Vector2 _screenPos;
    private float _startAngle;

    private float _indicatorDistance;
    private float _indicatorStartAngle;

    private float _buttonContainerDistance;
    private float _buttonStartAngle;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnDisable()
    {
        if (_isThisSelected) UnSelectCurrent();
    }

    private void Start()
    {
        _allObstacles.Add(this);
        _canvas.enabled = false;

        Vector2 indicatorVecToStartPos = _rotationIndicator.position - transform.position;
        _indicatorDistance = indicatorVecToStartPos.magnitude;
        _indicatorStartAngle = Mathf.Atan2(indicatorVecToStartPos.y, indicatorVecToStartPos.x);

        Vector2 buttonContainerVecToStartPos = _buttonsContainer.position - transform.position;
        _buttonContainerDistance = buttonContainerVecToStartPos.magnitude;
        _buttonStartAngle = Mathf.Atan2(buttonContainerVecToStartPos.y, buttonContainerVecToStartPos.x);
        
        _rotationHandle = GetComponentInChildren<RotationHandle>();
        if (_rotationHandle == null)
        {
            Debug.LogError(name + " no rotation handle found.");
        }
    }

    private void Update()
    {
        HandlePlacement();
        HandleUI();
    }

    public void OnTouchedDown(ToucheData touchData)
    {
        FingerInput fingerInput = InputManager.Instance.GetNewFingerAtPosAndTrack(touchData.screenPosition);
        _currentFinger = fingerInput.finger;
        _currentFingerState = FingerState.Moving;
    }

    public void OnTouchedUp(ToucheData touchData)
    {
        if (_currentFinger != null && _currentFinger.currentTouch.isTap)
        {
            Select();
        }
    }

    // ----- My Functions ----- \\

    private void HandleUI()
    {
        if (!_isThisSelected || !_canvas.enabled) return;

        Vector2 center = transform.position;
        Vector2 pos = center + Polar2Cart(_buttonStartAngle, _buttonContainerDistance);

        _buttonsContainer.position = pos;

        Vector3[] corners = new Vector3[4];
        _buttonsContainer.GetWorldCorners(corners);

        float leftX = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]).x;

        if (leftX < 0)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(new Vector3(-leftX, 0, 0)) - Camera.main.ScreenToWorldPoint(Vector3.zero);

            pos.x += difference.x;

            float angle = Mathf.Acos(((pos.x - center.x) / _buttonContainerDistance) % 1);
            pos.y = center.y + Mathf.Sin(angle) * _buttonContainerDistance;

            _buttonsContainer.position = pos;
        }
    }

    private void HandlePlacement()
    {
        if (_currentFinger == null) return;

        if (_currentFinger.currentTouch.ended)
        {
            _currentFinger = null;
            return;
        }

        switch (_currentFingerState)
        {
            case FingerState.Moving:
                MoveObstacle();
                break;
            case FingerState.Rotating:
                RotateObstacle();
                break;
        }
    }

    private void MoveObstacle()
    {
        if (_currentFinger.currentTouch.phase != UnityEngine.InputSystem.TouchPhase.Moved) return;

        Vector3 position = Camera.main.ScreenToWorldPoint(_currentFinger.screenPosition);
        position.z = 0;
        transform.position = position;
    }

    private void RefreshScreenPos()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        _screenPos = pos;
    }

    private void RotateObstacle()
    {
        Vector2 vecPosToStart = _currentFinger.currentTouch.startScreenPosition - _screenPos;
        Vector2 vecPosToCurrent = _currentFinger.currentTouch.screenPosition - _screenPos;
        
        float angle = Mathf.Atan2(vecPosToCurrent.y, vecPosToCurrent.x) - MathF.Atan2(vecPosToStart.y, vecPosToStart.x);
        _rotationIndicator.position = (Vector2)transform.position + Polar2Cart(angle + _indicatorStartAngle, _indicatorDistance);

        angle *= Mathf.Rad2Deg;
        angle += _startAngle;
        SetAngle(angle);
    }

    private Vector2 Polar2Cart(float angle, float distance)
    {
        return new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
    }

    static public void PickupObstacleWithFingerAtPos(Vector2 screenPos)
    {
        int length = _allObstacles.Count;
        for (int i = length - 1; i > -1; i--)
        {
            if (_allObstacles[i]._currentFinger != null && _allObstacles[i]._currentFinger.screenPosition == screenPos)
            {
                onObstaclePickedUp?.Invoke(_allObstacles[i]._obstacleType);
                Destroy(_allObstacles[i].gameObject);
                _allObstacles.RemoveAt(i);
            }
        }
    }

    public void SetFinger(Finger finger)
    {
        _currentFinger = finger;
    }

    private void OnRotationHandleTouched(ToucheData toucheData)
    {
        FingerInput fingerInput = toucheData.fingerInput;
        fingerInput.isTracked = true;
        _currentFinger = fingerInput.finger;
        _currentFingerState = FingerState.Rotating;
        RefreshScreenPos();
        _startAngle = GetAngle();
        _buttonsContainer.gameObject.SetActive(false);
    }

    private void OnFingerUp(Vector2 obj)
    {
        if (_currentFinger == null || _currentFinger.screenPosition != obj)
        {
            UnSelectCurrent();
            return;
        }

        _currentFinger = null;
        _buttonsContainer.gameObject.SetActive(true);
        _rotationIndicator.position = (Vector2)transform.position + Polar2Cart(_indicatorStartAngle, _indicatorDistance);
    }

    private void Select()
    {
        UnSelectCurrent();

        _hasAnObstacleSelected = true;
        _currentObstacleSelected = this;
        _canvas.enabled = true;
        _isThisSelected = true;

        InputManager.Instance.onFingerUp += OnFingerUp;
        _rotationHandle.onFingerDown += OnRotationHandleTouched;
    }

    private void UnSelect()
    {
        _hasAnObstacleSelected = false;
        _currentObstacleSelected = null;
        _canvas.enabled = false;
        _isThisSelected = false;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.onFingerUp -= OnFingerUp;
        }

        _rotationHandle.onFingerDown -= OnRotationHandleTouched;
    }

    static private void UnSelectCurrent()
    {
        if (!_hasAnObstacleSelected) return;

        _currentObstacleSelected.UnSelect();
    }

    public void Flip()
    {
        Vector3 scale = _mainObject.transform.localScale;
        scale.x *= -1;
        _mainObject.transform.localScale = scale;
    }

    public void SetAngle (float angle)
    {
        Vector3 rotation = _mainObject.transform.eulerAngles;
        rotation.z = angle;
        rotation.z %= 360;
        _mainObject.transform.eulerAngles = rotation;
    }

    public float GetAngle ()
    {
        return _mainObject.transform.eulerAngles.z;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}