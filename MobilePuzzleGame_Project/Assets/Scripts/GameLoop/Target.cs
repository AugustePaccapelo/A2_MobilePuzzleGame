using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Author : Auguste Paccapelo

public class Target : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    [SerializeField] private List<GameObject> _sprites = new();

    // ----- Objects ----- \\

    // ----- Events ----- \\
    public static event Action<int> OnWin;
    public static event Action<int> OnLoose;

    // ----- Others ----- \\

    [SerializeField] private int _id = 1;
    public int Id
    {
        get => _id;
        set
        {
            if (value < 1)
            {
                Debug.LogWarning(name + ": id cannot be less than 1.");
                _id = 1;
                return;
            }
            _id = value;
        }
    }
    [SerializeField] private LayerMask _layerToDestroy;

    private static Dictionary<int, Target> _mapTargets = new();

    private static int _numTargets = 0;
    private static int _numTargetsFinished = 0;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        Ball.onBallRespawn += OnBallRespawn;
    }

    private void OnDisable()
    {
        Ball.onBallRespawn -= OnBallRespawn;
    }

    private void OnValidate()
    {
        Id = _id;
        #if UNITY_EDITOR
            EditorApplication.delayCall += DelayFuncToShutUpUnity;
        #endif
    }

    private void Awake()
    {
        Id = _id;
        ChangeVisual();
        
        _numTargets++;
        _numTargetsFinished = 0;
        _mapTargets[_id] = this;
    }

    private void Start() { }

    private void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1 << go.layer => exact layerMask of go
        // if Note = 6 => 00100000
        if (((1 << collision.gameObject.layer) & _layerToDestroy) == 0) return;

        //Destroy(collision.gameObject);
        collision.gameObject.SetActive(false);

        GhostNote ghostComp;
        if (collision.TryGetComponent(out ghostComp))
        {
            return;
        }

        Ball note = collision.GetComponent<Ball>();

        if (note.Id == _id)
        {
            _numTargetsFinished++;
            OnWin?.Invoke(_id);

            if (_numTargetsFinished == _numTargets)
            {
                if (_numTargets >= 2)
                {
                    GooglePlayManager.CompleteAchievement(AchivementEnum.VoieDouble);
                }
                Debug.Log("OnWin");
                GameManager.Instance.FinishLevel();
            }
        }
        else
        {
            GameManager.Instance.RestartGame();
            _numTargetsFinished = 0;
            OnLoose?.Invoke(_id);
        }
    }

    // ----- My Functions ----- \\

    public static Target GetTargetId(int id)
    {
        return _mapTargets[id];
    }

    private void OnBallRespawn()
    {
        _numTargetsFinished = 0;
    }

    private void DelayFuncToShutUpUnity()
    {
        #if UNITY_EDITOR
            EditorApplication.delayCall -= DelayFuncToShutUpUnity;
        #endif
        if (this == null || gameObject == null) return;

        ChangeVisual();
    }

    private void ChangeVisual()
    {
        int length = _sprites.Count;
        for (int i = 0; i < length; i++)
        {
            _sprites[i].gameObject.SetActive(Id == i + 1);
        }
    }

    // ----- Destructor ----- \\

    private void OnDestroy()
    {
        _numTargets--;
        //if (_numTargets < 0) _numTargets = 0;
        _numTargetsFinished = 0;
    }
}