using UnityEngine;

// Author : Auguste Paccapelo

public class PhysicsComponent : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    private Rigidbody2D _rigidBody;

    // ----- Others ----- \\

    private bool _isSimulating = true;
    public bool IsSimulating => _isSimulating;

    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _gravityScale = 1.0f;

    [SerializeField] private Vector2 _gravityDirection = Vector2.down;

    [SerializeField] private float _mass = 1.0f;
    public float Mass
    {
        get => _mass;
        private set
        {
            if (value < 0)
            {
                _mass = 0f;
                Debug.LogWarning(name + ": mass cannot be negative.");
                return;
            }
            _mass = value;
        }
    }

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void Reset()
    {
        GetRigidBody();

        _gravity = 9.81f;
        _gravityScale = 1.0f;
        _gravityDirection = Vector2.down;
        _mass = 1.0f;
    }

    private void OnValidate()
    {
        Mass = _mass;
    }

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        if (_rigidBody == null) GetRigidBody();
    }

    private void Start() { }

    private void Update()
    {
        ApplyGravity();
    }

    // ----- My Functions ----- \\

    public void SetIsSimulating(bool isSimulating)
    {
        _isSimulating = isSimulating;
    }

    private void GetRigidBody()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogWarning(name + ": no rigidBody found.");
        }
    }

    private void ApplyGravity()
    {
        Vector2 gravityAcceleration = _gravityDirection * _gravity * _gravityScale;
        ApplyAcceleration(gravityAcceleration);
    }

    public void ApplyAcceleration(Vector2 acceleration)
    {
        _rigidBody.linearVelocity += acceleration * Time.deltaTime;
    }

    public void ApplyForce(Vector2 force)
    {
        Vector2 acceleration = force / _mass;
        ApplyAcceleration(acceleration);
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        _rigidBody.linearVelocity += impulse / _mass;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}