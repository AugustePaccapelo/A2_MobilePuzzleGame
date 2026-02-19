using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

// Author : Auguste Paccapelo

public class UIObstacleSpawner : MonoBehaviour, ITouchableOnDown, ITouchableOnUp
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    private const string OBSTACLE_DATA_PATH = "Data/";
    private const string OBSTACLE_DATA_FILE_NAME = "ObstacleData";
    private GameObject _prefabToSpawn;

    // ----- Objects ----- \\

    private ObstacleData _obstacleData;
    [SerializeField] private PlacableObstacle _obstacle;
    [SerializeField] private string _textPrefix = "";
    [SerializeField] private string _textSufix = "x";

    [SerializeField] private Text _numberText;
    [SerializeField] private Transform _obstaclesContainer;

    private ObstaclesPlacer _lastObstacle;
    private Finger _lastFinger;

    static private Dictionary<PlacableObstacle, Stack<GameObject>> _obstaclesPool = new();

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
        #if UNITY_EDITOR
            EditorApplication.delayCall += DelayFuncToShutUpUnity;
        #endif
        //DelayFuncToShutUpUnity();
    }

    private void OnEnable()
    {
        ObstaclesPlacer.onObstaclePickedUp += OnObstaclePickedUp;

        if (_obstacle == PlacableObstacle.Empty) return;

        GeneratePool();
    }

    private void GeneratePool()
    {
        GameObject platform;
        for (int i = 0; i < _numAllowedObstacle; i++)
        {
            platform = Instantiate(_prefabToSpawn, _obstaclesContainer);
            AddObstacleToPool(platform);
        }
    }

    private void OnDisable()
    {
        ObstaclesPlacer.onObstaclePickedUp -= OnObstaclePickedUp;

        if (!_obstaclesPool.ContainsKey(_obstacle)) return;

        _obstaclesPool.Remove(_obstacle);
    }

    private void Awake()
    {
        GetObstacleData();
        UpdateObstacle();

        if (_obstacle == PlacableObstacle.Empty) return;

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
        //if (_lastObstacle == null) return;
        //if (_lastFinger == null) return;

        //if (_lastFinger.currentTouch.valid)
        //{
        //    if (_lastFinger.currentTouch.ended)
        //    {
        //        if (_lastFinger.currentTouch.isTap)
        //        {
        //            _lastObstacle.Pickup();
        //        }

        //        _lastObstacle = null;
        //        _lastFinger = null;
        //    }
        //}
    }

    public void OnTouchedDown(ToucheData touchData)
    {
        //if (_obstacle == PlacableObstacle.Empty || _numAllowedObstacle <= 0 || GameManager.CurrentGameState != GameState.PlayerPlacingPlatforms) return;
        if (_obstacle == PlacableObstacle.Empty || _numAllowedObstacle <= 0) return;

        _numAllowedObstacle--;
        _numberText.text = _textPrefix + _numAllowedObstacle + _textSufix;
        touchData.fingerInput.isTracked = true;
        SpawnObstacle(touchData.worldPosition, touchData.fingerInput);
    }

    public void OnTouchedUp(ToucheData toucheData)
    {
        Ray ray = Camera.main.ScreenPointToRay(toucheData.screenPosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

        ObstaclesPlacer obstaclePlacer;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out obstaclePlacer))
            {
                obstaclePlacer.Pickup();
            }
        }
    }

    // ----- My Functions ----- \\

    private void AddObstacleToPool(GameObject go)
    {
        if (!_obstaclesPool.ContainsKey(_obstacle))
        {
            _obstaclesPool.Add(_obstacle, new());
        }

        _obstaclesPool[_obstacle].Push(go);
        go.SetActive(false);
        go.GetComponent<ObstaclesPlacer>().ResetState();
    }

    private GameObject GetObstacleFromPool()
    {
        if (!_obstaclesPool.ContainsKey(_obstacle)) return Instantiate(_prefabToSpawn, _obstaclesContainer);
        if (_obstaclesPool[_obstacle] == null) return Instantiate(_prefabToSpawn, _obstaclesContainer);
        if (_obstaclesPool[_obstacle].Count == 0) return Instantiate(_prefabToSpawn, _obstaclesContainer);
        
        GameObject go = _obstaclesPool[_obstacle].Pop();
        go.SetActive(true);

        return go;
    }

    private void DelayFuncToShutUpUnity()
    {
        #if UNITY_EDITOR
            EditorApplication.delayCall -= DelayFuncToShutUpUnity;
        #endif

        if (this == null || gameObject == null) return;

        GetObstacleData();
        UpdateObstacle();        
    }

    private void OnObstaclePickedUp(PlacableObstacle obj, GameObject go)
    {
        if (obj == _obstacle)
        {
            _numAllowedObstacle++;
            _numberText.text = _textPrefix + _numAllowedObstacle + _textSufix;
            AddObstacleToPool(go);
        }
    }

    private GameObject SpawnObstacle(Vector3 position, FingerInput fingerInput)
    {
        //GameObject obstacle = Instantiate(_prefabToSpawn, _obstaclesContainer);
        GameObject obstacle;

        obstacle = GetObstacleFromPool();
        obstacle.transform.position = position;

        ObstaclesPlacer obstaclesPlacer = obstacle.GetComponent<ObstaclesPlacer>();
        _lastObstacle = obstaclesPlacer;
        _lastFinger = fingerInput.finger;

        obstaclesPlacer.SetFinger(fingerInput.finger);
        obstaclesPlacer.Select();

        return obstacle;
    }

    private void GetObstacleData()
    {
        _obstacleData = Resources.Load<ObstacleData>(OBSTACLE_DATA_PATH + OBSTACLE_DATA_FILE_NAME);
    }

    private void UpdateObstacle()
    {
        ObstacleInfo obsInfo = _obstacleData.FindObstacleInfo(_obstacle);

        if (obsInfo == null)
        {
            Debug.LogError(name + ": didn't found ObstacleData");
            return;
        }

        _prefabToSpawn = obsInfo.prefabToPlace;

        Image icon;
        if (!TryGetComponent(out icon))
        {
            Debug.LogError(name + ": don't have a raw image component.");
            return;
        }

        icon.sprite = obsInfo.icon;

        _numberText.text = _textPrefix + _numAllowedObstacle + _textSufix;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}