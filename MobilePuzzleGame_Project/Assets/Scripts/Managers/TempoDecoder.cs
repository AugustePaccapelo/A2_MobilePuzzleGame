using System;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Décodeur Universel pour les plateformes
/// 
/// Author = Maxence Bernard
/// </summary>

public class TempoDecoder : MonoBehaviour
{

    [BoxGroup("GD")]
    [HorizontalLine(color: EColor.Blue)]
    [Label("Tempo de déclenchement"), SerializeField, MinValue(1), MaxValue(4)] private int _beatNumber;
    public int BeatNumber
    {
        get { return _beatNumber; }
        set { _beatNumber = value; SetBeforeAndAfter(); }
    }

    // -- Events --
    private event Action OnBeatBefore;
    private event Action OnBeat;
    private event Action OnBeatAfter;

    private int _beatBefore;
    private int _beatAfter;

    private void Start()
    {
        TempoManager.Instance.OnBeat += DecodeBeat;

        SetBeforeAndAfter();
    }

    // ----- Je viens set les beat précedent et beat suivant pour qu'ils restent dans la range 1 - 4
    [Button]
    private void SetBeforeAndAfter()
    {
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
        }
        else if (beatIndex == _beatAfter)
        {
            OnBeatAfter?.Invoke();
        }

    }

    private void OnDestroy()
    {
        TempoManager.Instance.OnBeat -= DecodeBeat;
    }

}
