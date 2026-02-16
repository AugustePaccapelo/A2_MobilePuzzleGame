using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CorThrowing : MonoBehaviour
{
    private TempoDecoder _tempoDecoder;
    [SerializeField] private UnityEvent onBallThrown; 
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwAngle = 45f;
    [SerializeField] private bool ballIn = false;
    [SerializeField] private LayerMask _targetLayers;
    private GameObject currentBall;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform _exitPoint;
    // bool to wait for the ball to fully exited before being able to reabsorbed it
    private bool _canPickupBall = true;
    private Transform _parent;

    void Start()
    {
        animator = GetComponent<Animator>();
        onBallThrown = new UnityEvent();
        
        _tempoDecoder = GetComponent<TempoDecoder>();
        if (_tempoDecoder != null)
        {
            _tempoDecoder.OnBeat += OnBeatReceived;
        }

        // Scrip is on MainObject, but it's Platfrom - Cor that is rotating
        _parent = transform.parent;
    }

    private void OnEnable()
    {
        Ball.onBallRespawn += OnBallRespawn;
    }

    private void OnDisable()
    {
        Ball.onBallRespawn -= OnBallRespawn;
    }

    private void OnBallRespawn()
    {
        ballIn = false;
        _canPickupBall = true;
        currentBall = null;
    }

    private void OnBeatReceived()
    {
        if (!ballIn) return;
        if (currentBall == null) return;

        ThrowBall(currentBall);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_canPickupBall) return;
        // 1 << go.layer => exact layerMask of go
        // if Note = 6 => 00100000
        if (((1 << other.gameObject.layer) & _targetLayers) != 0)
        {
            //other.GetComponent<SpriteRenderer>().enabled = false;
            _canPickupBall = false;
            other.gameObject.SetActive(false);
            StartCoroutine(HandleBallEnter(other));
        }
    }

    IEnumerator HandleBallEnter(Collider2D other)
    {
        currentBall = other.gameObject;

        LockBallPosition(currentBall);

        //var sr = other.GetComponent<SpriteRenderer>();
        //sr.enabled = false;
        other.gameObject.SetActive(false);

        //yield return new WaitForSeconds(0.5f);
        yield return null;
        ballIn = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (ballIn && ((1 << other.gameObject.layer) & _targetLayers) != 0)
        {
            ballIn = false;
            _canPickupBall = true;
            currentBall = null;
        }
    }

    public void ThrowBall(GameObject ball)
    {
        //ball.GetComponent<SpriteRenderer>().enabled = true;

        // If ball have moved
        if (ball.transform.position != transform.position)
        {
            ballIn = false;
            _canPickupBall = true;
            currentBall = null;
            return;
        }

        ball.SetActive(true);
        ball.transform.position = _exitPoint.position;

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        float angleInRadians = (throwAngle + _parent.eulerAngles.z) * Mathf.Deg2Rad;
        Vector2 throwDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;

        rb.linearVelocity = throwDirection * throwForce;
        
        onBallThrown?.Invoke();
    }

    void LockBallPosition(GameObject ball)
    {
        ball.transform.position = transform.position;
        //Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        //rb.linearVelocity = Vector2.zero;
        //rb.angularVelocity = 0f;
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }
}