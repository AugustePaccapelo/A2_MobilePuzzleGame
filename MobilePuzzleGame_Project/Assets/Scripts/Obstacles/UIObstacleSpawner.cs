using UnityEngine;
using UnityEngine.EventSystems;

// Author : Auguste Paccapelo

public class UIObstacleSpawner : MonoBehaviour, IPointerDownHandler
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private Transform _obstaclesContainer;

    // ----- Objects ----- \\

    private GameObject _currentObjectToPlace;
    private FingerInput _currentFinger;

    // ----- Others ----- \\

    [SerializeField] private int _numAllowedObstacle = 0;
    public int NumAllowedObstacle
    {
        get => _numAllowedObstacle;
        private set
        {
            if (value < 0)
            {
                Debug.LogWarning(name + ": num obstacle cannot be negative.");
                _numAllowedObstacle = 0;
                return;
            }
            _numAllowedObstacle = value;
        }
    }

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void Reset()
    {
        _numAllowedObstacle = 0;
    }

    private void OnValidate()
    {
        NumAllowedObstacle = _numAllowedObstacle;
    }

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        if (_prefabToSpawn == null)
        {
            Debug.LogWarning(name + ": prefab to spawn is null.");
        }
        if (_obstaclesContainer == null)
        {
            Debug.LogWarning(name + ": obstacles container is null.");
        }
    }

    private void Start() { }

    private void Update()
    {
        if (_currentFinger == null || _currentObjectToPlace == null) return;

        if (_currentFinger.finger.currentTouch.ended)
        {
            _currentFinger = null;
            _currentObjectToPlace = null;
        }
        else
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(_currentFinger.finger.screenPosition);
            pos.z = 0;
            _currentObjectToPlace.transform.position = pos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {   
        if (_numAllowedObstacle <= 0 || _prefabToSpawn == null || _obstaclesContainer == null) return;
        if (_currentFinger != null || _currentObjectToPlace != null) return;

        _numAllowedObstacle--;
        
        _currentObjectToPlace = Instantiate(_prefabToSpawn, _obstaclesContainer);
        _currentFinger = InputManager.Instance.GetNewFingerAtPos(eventData.position);
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}