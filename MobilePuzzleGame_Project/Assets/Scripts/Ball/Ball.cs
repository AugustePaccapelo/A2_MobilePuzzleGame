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
                SetSprite();
                return;
            }
            _id = value;
            SetSprite();
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnParticules(collision);
        PlayCollisionSFX(collision);
        if (_lastObsHited != collision.gameObject)
        {
            _lastObsHited = collision.gameObject;
            ExecuteTween(collision);
        }
    }

    private void Update()
    {
        _renderer.transform.eulerAngles = Vector3.zero;
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

    private void PlayCollisionSFX(Collision2D col)
    {
        int colLayer = 1 << col.gameObject.layer;
        if ((colLayer & _obstaclesLayer) == 0) return;
        
        if (AudioManager.Instance == null || AudioManager.Instance.sfxClips.Count == 0) return;
        
        if (col.gameObject.tag == "Acordeon")
        {
            AudioManager.Instance.sfxClips[0].Play();
        }
        else if (col.gameObject.tag == "RainSticks")
        {
            AudioManager.Instance.sfxClips[3].Play();
        }
        else if (col.gameObject.tag == "Drum")
        {
            AudioManager.Instance.sfxClips[4].Play();
        }
        else if (col.gameObject.tag == "triangle")
        {
            AudioManager.Instance.sfxClips[5].Play();
        }
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