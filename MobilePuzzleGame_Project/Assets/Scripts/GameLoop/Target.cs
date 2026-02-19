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
    public event Action OnWin;
    public event Action OnLoose;

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

    private static int _numTargets = 0;
    private static int _numTargetsFinished = 0;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

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
            if (_numTargetsFinished == _numTargets)
            {
                if (_numTargets >= 2)
                {
                    GooglePlayManager.CompleteAchievement(AchivementEnum.VoieDouble);
                }
                GameManager.Instance.FinishLevel();
                OnWin?.Invoke();
            }
        }
        else
        {
            GameManager.Instance.RestartGame();
            _numTargetsFinished = 0;
            OnLoose?.Invoke();
        }
    }

    // ----- My Functions ----- \\

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
        if (_numTargets < 0) _numTargets = 0;
        _numTargetsFinished = 0;
    }
}