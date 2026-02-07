using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CorThrowing : MonoBehaviour
{
    private TempoDecoder _tempoDecoder;
    UnityEvent onBallThrown; 
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwAngle = 45f;
    [SerializeField] private bool ballIn = false;
    private int targetLayer;
    private GameObject currentBall;
    [SerializeField] private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetLayer = LayerMask.NameToLayer("Note");
        onBallThrown = new UnityEvent();
        
        if (_tempoDecoder != null)
        {
            _tempoDecoder.OnBeat += OnBeatReceived;
        }
    }

    private void OnBeatReceived()
    {
        if (!ballIn) return;
        if (currentBall == null) return;

        ThrowBall(currentBall);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            other.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(HandleBallEnter(other));
        }
    }

    IEnumerator HandleBallEnter(Collider2D other)
    {
        ballIn = true;
        currentBall = other.gameObject;

        LockBallPosition(currentBall);

        var sr = other.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        yield return new WaitForSeconds(0.5f);
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            ballIn = false;
        }
    }

    public void ThrowBall(GameObject ball)
    {
        ball.GetComponent<SpriteRenderer>().enabled = true;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        float angleInRadians = throwAngle * Mathf.Deg2Rad;
        Vector2 throwDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
        rb.linearVelocity = throwDirection * throwForce;
        ballIn = false;
        onBallThrown?.Invoke();
    }

    void LockBallPosition(GameObject ball)
    {
        ball.transform.position = transform.position;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
