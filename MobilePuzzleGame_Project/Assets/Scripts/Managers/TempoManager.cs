using NaughtyAttributes;
using System;
using UnityEngine;

// Author = Maxence Bernard

public class TempoManager : MonoBehaviour
{
    // ----- Variables ------
    [BoxGroup("GD"), HorizontalLine(color: EColor.Blue)]

    [Header("Tempo")]
    [Label("Tempo en bpm") ,SerializeField] private int _bpm;

    // --- Time ---
    private float _time;

    // --- Beat ---
    private float _beatTempo;
    public float BeatTempo => _beatTempo;
    private int _beatIndex;
    public int BeatIndex
    {
        get { return _beatIndex; }
        set
        {
            if (value > 4)
            {
                _beatIndex = 1;
            }
            else
            {
                _beatIndex = value;
            }
        }
    }

    // --- Event ---
    public event Action<int> OnBeat;

    // --- Instance ---
    public static TempoManager Instance;

    private void Awake()
    {
        _beatTempo = 60f / _bpm;
        _beatIndex = 1;

        Instance = this;
    }

    private void Update()
    {
        
        _time += Time.deltaTime;

        if (_time >= _beatTempo)
        {
            BeatIndex++;
            OnBeat?.Invoke(BeatIndex);
            _time -= _beatTempo;
        }

    }

    public void ResetTime()
    {
        _time = _beatTempo / 2;
        _beatIndex = 1;
    }
}
