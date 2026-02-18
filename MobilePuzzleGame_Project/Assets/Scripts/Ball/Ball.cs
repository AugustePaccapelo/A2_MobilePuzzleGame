using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private int _id = 1;
    [SerializeField] private List<Sprite> _sprites = new();
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private LayerMask _obstaclesLayer;
    [SerializeField] private ParticleSystem _collisionParticulePrefab;

    [SerializeField] private float _scaleDownOnBounceMultiplier;
    [SerializeField] private float _bounceEffectDuration;

    static public event Action onBallRespawn;

    private TweenCore _lastTween;

    static private GameObject _lastObsHited;

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
        #if UNITY_EDITOR
            EditorApplication.delayCall += DelayFuncToShutUpUnity;
        #endif
    }

    private void Awake()
    {
        Id = _id;
        SetSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnParticules(collision);
        if (_lastObsHited != collision.gameObject)
        {
            _lastObsHited = collision.gameObject;
            ExecuteTween(collision);
        }
    }

    private void SpawnParticules(Collision2D col)
    {
        if (_collisionParticulePrefab == null) return;
        int colLayer = 1 << col.gameObject.layer;
        if ((colLayer & _obstaclesLayer) == 0) return;
        ParticleSystem particules = Instantiate(_collisionParticulePrefab);
        particules.transform.position = col.contacts[0].point;
        particules.Play();
    }

    private void ExecuteTween(Collision2D col)
    {
        if (_lastTween != null)
        {
            _lastTween.Stop();
            _lastTween = null;
        }

        TweenCore tween = TweenCore.CreateTween();

        tween.SetParallel(false);

        Vector3 downScaleDirection = Vector2.Perpendicular(col.contacts[0].normal.normalized);
        //Vector3 downScaleDirection = col.contacts[0].normal.normalized;
        //Vector3 finalVal = downScaleDirection * _scaleDownOnBounceMultiplier;
        Vector3 finalVal = _renderer.transform.localScale;

        finalVal -= downScaleDirection * _scaleDownOnBounceMultiplier;

        tween.NewProperty(BounceTweenFunc, _renderer.transform.localScale, finalVal, _bounceEffectDuration * 0.5f);
        tween.NewProperty(BounceTweenFunc, finalVal, _renderer.transform.localScale, _bounceEffectDuration * 0.5f);

        _lastTween = tween;

        tween.Play();
    }

    private void BounceTweenFunc(Vector3 value)
    {
        _renderer.transform.localScale = value;
    }

    static public void TriggerOnBallRespawn()
    {
        
        onBallRespawn?.Invoke();
        _lastObsHited = null;
    }

    private void DelayFuncToShutUpUnity()
    {
        #if UNITY_EDITOR
            EditorApplication.delayCall -= DelayFuncToShutUpUnity;
        #endif
        if (this == null || gameObject == null) return;

        SetSprite();
    }

    private void SetSprite()
    {
        if (_sprites.Count < 0)
        {
            Debug.LogWarning(name + ": no sprites were given.");
            return;
        }

        if (_id <= _sprites.Count)
        {
            _renderer.sprite = _sprites[_id - 1];
        }
        else
        {
            Debug.LogError(name + ": key id not in the sprites.");
            _renderer.sprite = _sprites[0];
        }
    }

    private void OnDestroy()
    {
        if (_lastTween != null) _lastTween.Stop(false);
    }
}