using UnityEngine;
using UnityEngine.Events;

public class AccordeonCoil : MonoBehaviour
{
    [SerializeField] private TempoSubscriber tempoSubscriber;
    [SerializeField] private int[] activationBeats = { 1, 2, 3, 4 }; 
    [SerializeField] private float maxActivationValue = 1f;
    
    UnityEvent onCoilRest;
    UnityEvent onCoilActivated;
    UnityEvent onCoilDeactivated;
    UnityEvent onCoilHeld;
    
    private float currentActivation = 0f;
    private int activationIndex = 0;

    void Start()
    {
        onCoilRest = new UnityEvent();
        onCoilActivated = new UnityEvent();
        onCoilDeactivated = new UnityEvent();
        onCoilHeld = new UnityEvent();

        if (tempoSubscriber != null)
        {
            tempoSubscriber.onBeat.AddListener(OnBeatReceived);
        }
    }

    public void OnBeatReceived(int beatIndex)
    {
        if (activationIndex < activationBeats.Length && beatIndex == activationBeats[activationIndex])
        {
            currentActivation = (activationIndex + 1) / (float)activationBeats.Length * maxActivationValue;
            onCoilActivated?.Invoke();
            activationIndex++;
            
            if (activationIndex >= activationBeats.Length)
            {
                onCoilHeld?.Invoke();
            }
        }
    }

    void Update()
    {
        
    }

    public float GetActivationValue()
    {
        return currentActivation;
    }

    public void ResetCoil()
    {
        currentActivation = 0f;
        activationIndex = 0;
        onCoilRest?.Invoke();
    }
}
