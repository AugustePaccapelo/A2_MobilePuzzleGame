using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TempoSubscriber : MonoBehaviour
{
    public UnityEvent<int> onBeat = new UnityEvent<int>();

    private Coroutine _waitRoutine;

    private void OnEnable()
    {
        _waitRoutine = StartCoroutine(WaitForTempoManager());
    }

    private IEnumerator WaitForTempoManager()
    {
        while (TempoManager.Instance == null)
            yield return null;

        TempoManager.Instance.OnBeat += HandleBeat;
    }

    private void OnDisable()
    {
        if (_waitRoutine != null)
            StopCoroutine(_waitRoutine);

        if (TempoManager.Instance != null)
            TempoManager.Instance.OnBeat -= HandleBeat;
    }

    private void HandleBeat(int beatIndex)
    {
        onBeat.Invoke(beatIndex);
    }
}
