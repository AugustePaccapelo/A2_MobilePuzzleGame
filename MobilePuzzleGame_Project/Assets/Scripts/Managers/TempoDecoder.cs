using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Décodeur Universel pour les plateformes
/// 
/// Author = Maxence Bernard
/// </summary>

public class TempoDecoder : MonoBehaviour
{

    [BoxGroup("GD")]
    [HorizontalLine(color: EColor.Blue)]
    [Label("Tempo de déclenchement"), SerializeField, MinValue(0), MaxValue(4)] private int _beatNumber = 0;
    public int BeatNumber
    {
        get { return _beatNumber; }
        set
        { 
            if (value < 0)
            {
                _beatNumber = 4;
            }
            else if (value > 4)
            {
                _beatNumber = 0;
            }
            else
            {
                _beatNumber = value;
            }
            SetBeforeAndAfter();
        }
    }

    // -- Events --
    public event Action OnBeatBefore;
    public event Action OnBeat;
    public event Action OnBeatAfter;
    public event Action OnOffBeat;

    [SerializeField] private UnityEvent _unityOnBeat;
    [SerializeField] private UnityEvent _unityOnOffBeat;

    private int _beatBefore;
    private int _beatAfter;

    private void Start()
    {
        TempoManager.Instance.OnBeat += DecodeBeat;

        SetBeforeAndAfter();
    }

    private void OnValidate()
    {
        BeatNumber = _beatNumber;
    }

    // ----- Je viens set les beat précedent et beat suivant pour qu'ils restent dans la range 1 - 4
    [Button]
    private void SetBeforeAndAfter()
    {
        // 0 va être pour ceux qui n'ont pas de tempo d'activation
        if (_beatNumber == 0)
        {
            _beatBefore = _beatAfter = 0;
            return;
        }

        if (_beatNumber <= 1)
        {
            _beatBefore = 4;
        }
        else
        {
            _beatBefore = _beatNumber - 1;
        }

        if (_beatNumber >= 4)
        {
            _beatAfter = 1;
        }
        else
        {
            _beatAfter = _beatNumber + 1;
        }
    }

    private void DecodeBeat(int beatIndex)
    {
        if (beatIndex == _beatBefore)
        {
            OnBeatBefore?.Invoke();
        }
        else if (beatIndex == _beatNumber)
        {
            OnBeat?.Invoke();
            _unityOnBeat?.Invoke();
            return;
        }
        else if (beatIndex == _beatAfter)
        {
            OnBeatAfter?.Invoke();
        }

        if (beatIndex != _beatNumber)
        {
            OnOffBeat?.Invoke();
            _unityOnOffBeat?.Invoke();
        }
    }

    private void OnDestroy()
    {
        TempoManager.Instance.OnBeat -= DecodeBeat;
    }

}
