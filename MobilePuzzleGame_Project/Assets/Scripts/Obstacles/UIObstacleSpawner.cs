using UnityEngine;
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

    private void OnEnable()
    {
        ObstaclesPlacer.onObstaclePickedUp += OnObstaclePickedUp;
    }

    private void OnDisable()
    {
        ObstaclesPlacer.onObstaclePickedUp -= OnObstaclePickedUp;
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

    public void OnTouchedDown(ToucheData touchData)
    {
        if (_obstacle == PlacableObstacle.Empty || _numAllowedObstacle <= 0) return;

        _numAllowedObstacle--;
        _numberText.text = _textPrefix + _numAllowedObstacle + _textSufix;
        touchData.fingerInput.isTracked = true;
        SpawnObstacle(touchData.screenPosition, touchData.fingerInput);
    }

    public void OnTouchedUp(ToucheData toucheData)
    {
        ObstaclesPlacer.PickupObstacleWithFingerAtPos(toucheData.screenPosition);
    }

    // ----- My Functions ----- \\

    private void OnObstaclePickedUp(PlacableObstacle obj)
    {
        if (obj == _obstacle)
        {
            _numAllowedObstacle++;
            _numberText.text = _textPrefix + _numAllowedObstacle + _textSufix;
        }
    }

    private GameObject SpawnObstacle(Vector3 position, FingerInput fingerInput)
    {
        if (GameManager.CurrentGameState != GameState.PlayerPlacingPlatforms) return null;

        GameObject obstacle = Instantiate(_prefabToSpawn, _obstaclesContainer);
        obstacle.transform.position = position;
        ObstaclesPlacer obstaclesPlacer = obstacle.GetComponent<ObstaclesPlacer>();
        
        obstaclesPlacer.SetFinger(fingerInput.finger);

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