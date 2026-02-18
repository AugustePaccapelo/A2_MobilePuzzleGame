using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Acordeon : MonoBehaviour
{
    #region AcordeonState
    public enum AcordeonState
    {
        Prepare,
        Extend,
        Retract
    }

    private AcordeonState _state;
    public AcordeonState State => _state;
    #endregion

    #region GD Vairables
    [HorizontalLine(color: EColor.Blue)]
    [BoxGroup("GD"), Label("Distance d'extention"), SerializeField] private float _extendDistance;

    [Header("Dur�e d'animation")]
    [BoxGroup("GD"), Label("Preparation"), SerializeField] private float _prepareDuration;
    [BoxGroup("GD"), Label("Extension"), SerializeField] private float _extendDuration;
    [BoxGroup("GD"), Label("Retract"), SerializeField] private float _retractDuration;

    [Header("Puissance d'�jection")]
    [BoxGroup("GD"), Label("Puissance"), SerializeField] private float _power;
    public float Power => _power;

    [Header("Events")]
    [BoxGroup("GD"), SerializeField] private UnityEvent OnUse;
    #endregion

    #region GP Variables
    [BoxGroup("GP - Pas touche ! �_�")]
    [HorizontalLine(color: EColor.Violet)]
    [SerializeField] private Transform _topPart;

    [BoxGroup("GP - Pas touche ! �_�")]
    [SerializeField] private Transform _bottomPart;
    #endregion

    private bool _actionDone;
    public bool ActionDone
    {
        get => _actionDone;
        set => _actionDone = value;
    }

    Vector2 _startingTopPosition;

    private void Awake()
    {
        TempoDecoder decoder = GetComponent<TempoDecoder>();

        decoder.OnBeatBefore += PrepareState;
        decoder.OnBeat += ExtendState;
        decoder.OnBeatAfter += RetractState;

        _startingTopPosition = _topPart.localPosition;

    }

    private void OnDestroy()
    {
        TempoDecoder decoder = GetComponent<TempoDecoder>();

        decoder.OnBeatBefore -= PrepareState;
        decoder.OnBeat -= ExtendState;
        decoder.OnBeatAfter -= RetractState;
    }

    IEnumerator MoveObject(Vector2 startPosition, Vector2 endPosition, float duration)
    {
        float time = 0;
        float ratio;

        _actionDone = false;

        while (time < duration)
        {
            time += Time.deltaTime;
            ratio = time / duration;

            _topPart.localPosition = Vector2.Lerp(startPosition, endPosition, ratio);
            yield return null;

        }

        _actionDone = true;
        yield break;
    }

    private void PrepareState()
    {
        _state = AcordeonState.Prepare;
        StartCoroutine(MoveObject(_topPart.localPosition, (_bottomPart.localPosition - _topPart.localPosition) * .25f, _prepareDuration));
    }

    private void ExtendState()
    {
        _state = AcordeonState.Extend;
        StartCoroutine(MoveObject(new Vector2(0 , _topPart.localPosition.y), _bottomPart.localPosition + Vector3.up * _extendDistance , _extendDuration));
        OnUse.Invoke();
    }

    private void RetractState()
    {
        _state = AcordeonState.Retract;
        StartCoroutine(MoveObject(_topPart.localPosition, _startingTopPosition, _retractDuration));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_bottomPart.position, .25f);
        Gizmos.DrawLine(_bottomPart.position, _bottomPart.position + transform.up * _extendDistance);
        Gizmos.DrawSphere(_bottomPart.position + transform.up * _extendDistance, .25f);
    }

}
