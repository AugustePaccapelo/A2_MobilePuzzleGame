using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private int _id = 1;
    [SerializeField] private LayerMask _obstaclesLayer;
    [SerializeField] private ParticleSystem _collisionParticulePrefab;

    public int Id
    {
        get => _id;
        set
        {
            if (value < 1)
            {
                Debug.LogWarning(name + ": id cannot be less than 1.");
                _id = 1;
                return;
            }
            _id = value;
        }
    }

    private void OnValidate()
    {
        Id = _id;
    }

    private void Awake()
    {
        Id = _id;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnParticules(collision);
    }

    private void SpawnParticules(Collision2D col)
    {
        if (_collisionParticulePrefab == null) return;
        if (((1 << col.gameObject.layer) & _obstaclesLayer) == 0) return;
        ParticleSystem _particules = Instantiate<ParticleSystem>(_collisionParticulePrefab);
        _particules.transform.position = col.contacts[0].point;
        _particules.Play();
    }
}