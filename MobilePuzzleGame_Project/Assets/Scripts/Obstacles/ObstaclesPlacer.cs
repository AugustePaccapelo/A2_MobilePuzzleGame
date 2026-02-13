using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

// Author : Auguste Paccapelo & Maxence Bernard

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
    [SerializeField] private GameObject _buttonTempo;

    [SerializeField] private RectTransform _rotationIndicator;

    [SerializeField] private Text _tempoText;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _renderer;

    private Finger _currentFinger;
    public Finger CurrentFinger => _currentFinger;

    private RotationHandle _rotationHandle;
    private TempoDecoder _tempoDecoder;

    private BoxCollider2D _boxCollider;

    // ----- Boolean ----- \\
    [SerializeField] private bool _isBasedOnTempo = false;
    [SerializeField] private bool _stickToWall;

    // ----- Events ----- \\

    static public event Action<PlacableObstacle, GameObject> onObstaclePickedUp;

    // ----- Others ----- \\

    [SerializeField, EnableIf("_stickToWall")] private LayerMask _stickToWallLayer;

    [SerializeField] private PlacableObstacle _obstacleType;

    [SerializeField] private List<float> _allowedRotations = new List<float>();

    [SerializeField] private Color _nonPlacableColorFeedBack = Color.red;

    private bool _isThisSelected = false;
    public bool IsThisSelected => _isThisSelected;

    private bool _canBePlaced = true;
    private int _numObjetsInCollider = 0;

    static private bool _hasAnObstacleSelected = false;
    static public bool HasAnObstacleSelected => _hasAnObstacleSelected;
    static public event Action onObstacleSelected;
    static public event Action onObstacleUnselected;

    static private ObstaclesPlacer _currentObstacleSelected;
    static public ObstaclesPlacer CurrentObstacleSelected => _currentObstacleSelected;
    private FingerState _currentFingerState;

    private Vector2 _screenPos;
    private float _startAngle;

    private float _indicatorDistance;
    private float _indicatorStartAngle;

    private float _buttonContainerDistance;
    private float _buttonStartAngle;

    private bool _hasBeenPlaced = false;
    private Vector2 _lastPos;
    private float _lastAngle;

    #region StickingToWall Variables
    // -- Position la plus proche --
    private Vector3 _closestPosition;

    // -- Vecteur de direction --
    private Vector2 _direction = Vector2.up;
    private Vector3 _normal;

    #endregion

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnDisable()
    {
        if (_isThisSelected) UnSelectCurrent();
    }

    private void OnEnable()
    {
        _canvas.enabled = false;
        _canvas.worldCamera = Camera.main;

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

        if (!_isBasedOnTempo) _buttonTempo.SetActive(false);
        else ChangeTempo();

        _canBePlaced = true;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _tempoDecoder = GetComponentInChildren<TempoDecoder>();
        if (_tempoDecoder == null)
        {
            Debug.LogWarning(name + ": no tempo decoder found.");
        }

        _renderer = GetComponentInChildren<SpriteRenderer>();

        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        CheckCollider();
        HandlePlacement();
        HandleUI();
    }

    public void OnTouchedDown(ToucheData touchData)
    {
        FingerInput fingerInput = InputManager.Instance.GetNewFingerAtPosAndTrack(touchData.screenPosition);
        if (fingerInput == null) return;

        _currentFinger = fingerInput.finger;
        _currentFingerState = FingerState.Moving;
    }

    public void OnTouchedUp(ToucheData touchData)
    {
        if (_currentFinger != null)
        {
            //if (_currentFinger.currentTouch.isTap)
            //{
            //    Select();
            //    return;
            //}
            Select();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //_numObjetsInCollider++;
        //_canBePlaced = false;
        //_renderer.color = _nonPlacableColorFeedBack;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //_numObjetsInCollider--;
        //if (_numObjetsInCollider == 0)
        //{
        //    _canBePlaced = true;
        //    _renderer.color = Color.white;
        //}
    }

    // ----- My Functions ----- \\

    private void CheckCollider()
    {
        List<Collider2D> colliders = new();
        _boxCollider.GetContacts(colliders);
        _numObjetsInCollider = colliders.Count;

        if (_numObjetsInCollider > 0)
        {
            _canBePlaced = false;
            _renderer.color = _nonPlacableColorFeedBack;
        }
        else
        {
            _canBePlaced = true;
            _renderer.color = Color.white;
        }
    }

    public void ResetState()
    {
        _currentFinger = null;
        _hasBeenPlaced = false;
        _numObjetsInCollider = 0;
        _canBePlaced = true;
        _renderer.color = Color.white;
        _isThisSelected = false;
        _canvas.enabled = false;
        SetAngle(0);
    }

    private bool IsPosInButtons(Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_buttonsContainer, pos);
    }

    private void HandleUI()
    {
        if (!_isThisSelected || !_canvas.enabled) return;

        Vector2 center = transform.position;

        // Buttons handling
        Vector2 buttonPos = center + Polar2Cart(_buttonStartAngle, _buttonContainerDistance);

        _buttonsContainer.position = buttonPos;

        Vector3[] buttonCorners = new Vector3[4];
        _buttonsContainer.GetWorldCorners(buttonCorners);

        float buttonLeftX = RectTransformUtility.WorldToScreenPoint(Camera.main, buttonCorners[0]).x;

        Vector3 rotation = _buttonsContainer.eulerAngles;
        rotation.z = 0;
        _buttonsContainer.eulerAngles = rotation;

        if (buttonLeftX < 0)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(new Vector3(buttonLeftX, 0, 0)) - Camera.main.ScreenToWorldPoint(Vector3.zero);

            buttonPos.x -= difference.x;

            float angle = Mathf.Acos(((buttonPos.x - center.x) / _buttonContainerDistance) % 1);
            buttonPos.y = center.y + Mathf.Sin(angle) * _buttonContainerDistance;

            _buttonsContainer.position = buttonPos;

            rotation.z = Mathf.Rad2Deg * -(_buttonStartAngle - angle);
            _buttonsContainer.eulerAngles = rotation;
        }

        // Rotation Handle handling
        if (_currentFingerState == FingerState.Rotating) return;

        Vector2 rotationHandlePos = center + Polar2Cart(_indicatorStartAngle, _indicatorDistance);
        
        _rotationIndicator.transform.position = rotationHandlePos;

        Vector3[] rotationHandleCorners = new Vector3[4];
        _rotationIndicator.GetWorldCorners(rotationHandleCorners);

        float rotationHandleRightX = RectTransformUtility.WorldToScreenPoint(Camera.main, rotationHandleCorners[3]).x;

        if (rotationHandleRightX > Screen.currentResolution.width)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(new Vector3(rotationHandleRightX, 0, 0)) - Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, 0, 0));

            rotationHandlePos.x -= difference.x;

            // -Acos to rotation clockwise
            float angle = -Mathf.Acos(((rotationHandlePos.x - center.x) / _indicatorDistance) % 1);
            rotationHandlePos.y = center.y + Mathf.Sin(angle) * _indicatorDistance;
            
            _rotationIndicator.transform.position = rotationHandlePos;
        }
    }

    private void HandlePlacement()
    {
        //if (_currentFinger == null || GameManager.CurrentGameState != GameState.PlayerPlacingPlatforms) return;
        if (_currentFinger == null) return;

        if (_currentFinger.currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
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
        //transform.position = position;
        _rigidBody.MovePosition(position);
    }

    private void PositionStickingObstacle()
    {
        for (int i = 0; i < 8; i++)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 100f, _stickToWallLayer);
            if (hit.collider == null)
            {
                _direction = Portal.RotateVector2(_direction, 45 * Mathf.Deg2Rad);
                continue;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                if (Vector2.Distance(transform.position, hit.point) < Vector2.Distance(transform.position, _closestPosition) || _closestPosition == Vector3.zero)
                {
                    _closestPosition = hit.point;
                    _normal = hit.normal;
                }

            }

            //On tourne la direction à 45 degré pour avoir les 8 raycast autour du portail
            _direction = Portal.RotateVector2(_direction, 45 * Mathf.Deg2Rad);

            Debug.DrawLine(transform.position, transform.position + (Vector3)_direction);
        }

        transform.position = _closestPosition + (transform.position - _closestPosition).normalized * GetComponentInChildren<SpriteRenderer>().transform.localScale.x;
        transform.up = _normal;
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
        // Keep angle between 0 - 360
        
        if (_allowedRotations.Count != 0) angle = GetCLosestAngleInList((angle + 360) % 360);
        SetAngle(angle);
    }

    private float GetCLosestAngleInList(float angle)
    {
        float closest = _allowedRotations[0];
        float minDifference = Mathf.Abs(angle - closest);

        float currentDifference;

        int length = _allowedRotations.Count;
        for (int i = 1; i < length; i++)
        {
            currentDifference = Mathf.Abs(angle - _allowedRotations[i]);
            if (currentDifference < minDifference)
            {
                closest = _allowedRotations[i];
                minDifference = currentDifference;
            }
        }
        
        return closest;
    }

    private Vector2 Polar2Cart(float angle, float distance)
    {
        return new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
    }

    public void Pickup()
    {
        UnSelect();
        onObstaclePickedUp?.Invoke(_obstacleType, gameObject);
    }

    public void SetFinger(Finger finger)
    {
        _currentFinger = finger;
        _currentFingerState = FingerState.Moving;
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
        // If click on a button
        if (IsPosInButtons(Camera.main.ScreenToWorldPoint(obj)))
        {
            return;
        }

        if (_currentFinger == null || _currentFinger.screenPosition != obj)
        {
            UnSelectCurrent();
            return;
        }
        
        if (!_canBePlaced)
        {
            if (!_hasBeenPlaced)
            {
                Pickup();
            }
            else
            {
                _rigidBody.MovePosition(_lastPos);
                Vector3 angle = transform.eulerAngles;
                angle.z = _lastAngle;
                transform.eulerAngles = angle;
            }
        }
        else
        {
            _hasBeenPlaced = true;
            _lastPos = transform.position;
            _lastAngle = transform.eulerAngles.z;
        }

        
        if (_stickToWall)
        {
            PositionStickingObstacle();
        }

        _currentFinger = null;
        _buttonsContainer.gameObject.SetActive(true);
        _rotationIndicator.position = (Vector2)transform.position + Polar2Cart(_indicatorStartAngle, _indicatorDistance);
    }

    public void Select()
    {
        Portal newExitPortal = default;
        if (_hasAnObstacleSelected)
        {
            if (_currentObstacleSelected.GetComponentInChildren<Portal>() != null)
            {
                newExitPortal = _currentObstacleSelected.GetComponentInChildren<Portal>();
            }
        }

        UnSelectCurrent();

        _hasAnObstacleSelected = true;
        _currentObstacleSelected = this;
        _canvas.enabled = true;
        _isThisSelected = true;

        InputManager.Instance.onFingerUp += OnFingerUp;
        _rotationHandle.onFingerDown += OnRotationHandleTouched;

        onObstacleSelected?.Invoke();

        if (newExitPortal != null)
        {
            if (_currentObstacleSelected.GetComponentInChildren<Portal>() != null)
            {
                _currentObstacleSelected.GetComponentInChildren<Portal>().SetExitPortal(newExitPortal);
            }
        }
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

        onObstacleUnselected?.Invoke();
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
        Vector3 rotation = transform.eulerAngles;
        rotation.z = angle;
        transform.eulerAngles = rotation;
        _canvas.transform.eulerAngles = Vector3.zero;
    }

    public float GetAngle ()
    {
        return transform.eulerAngles.z;
    }

    private void UpdateTempo()
    {
        _tempoText.text = _tempoDecoder.BeatNumber.ToString();
    }

    public void ChangeTempo()
    {
        // Skip 0
        if (_tempoDecoder.BeatNumber == 4) _tempoDecoder.BeatNumber = 1;
        else  _tempoDecoder.BeatNumber++;
        UpdateTempo();
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}