using UnityEngine;
using UnityEngine.Events;

public class DrumEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent OnBounce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBounce.Invoke();
    }

}
