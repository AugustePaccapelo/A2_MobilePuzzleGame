using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

// Author : Auguste Paccapelo

public class UIObstacleSpawner : MonoBehaviour, IPointerDownHandler, ITouchableOnUp
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    private ObstacleData _obstacleData;
    [SerializeField] private PlacableObstacle _obstacle;
    private ObstacleInfo _obstacleInfo;
    private GameObject _prefabToSpawn;
    [SerializeField] private Transform _obstaclesContainer;

    // ----- Objects ----- \\

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
        GetObstacleData();
        UpdateObstacle();
    }

    private void OnValidate()
    {
        NumAllowedObstacle = _numAllowedObstacle;
        GetObstacleData();
        UpdateObstacle();
    }

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        GetObstacleData();
        
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

    private void Update() { }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_numAllowedObstacle <= 0 || _prefabToSpawn == null || _obstaclesContainer == null) return;

        _numAllowedObstacle--;

        GameObject obstacle = Instantiate(_prefabToSpawn, _obstaclesContainer);
        Vector3 pos = Camera.main.ScreenToWorldPoint(transform.position);
        pos.z = 0;
        obstacle.transform.position = pos;
        
        FingerInput finger = InputManager.Instance.GetNewFingerAtPos(eventData.position);
        ObstaclesPlacer placer = obstacle.GetComponent<ObstaclesPlacer>();
        placer.Creator = this;
        ObstaclesPlacer.UnSelectCurrent();
        placer.Select(finger);
    }

    public void OnTouchedUp(Vector2 screenPos)
    {
        ObstaclesPlacer.PickupCurrentObstacle();
    }

    // ----- My Functions ----- \\

    private void GetObstacleData()
    {
        _obstacleData = ObstacleData.instance;
    }

    private void UpdateObstacle()
    {
        ObstacleInfo obsInfo = _obstacleData.FindObstacleInfo(_obstacle);

        if (obsInfo == null)
        {
            return;
        }

        _prefabToSpawn = obsInfo.prefabToPlace;

        RawImage icon;
        if (!TryGetComponent(out icon))
        {
            Debug.LogError(name + ": don't have a raw image componenet.");
            return;
        }

        icon.texture = obsInfo.icon;
    }

    public void PickupObstacle(GameObject obstacle)
    {
        _numAllowedObstacle++;
        Destroy(obstacle);
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}