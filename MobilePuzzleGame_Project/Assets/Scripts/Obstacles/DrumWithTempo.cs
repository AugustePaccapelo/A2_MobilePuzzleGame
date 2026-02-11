using UnityEngine;

// Author : Auguste Paccapelo

public class DrumWithTempo : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private BoxCollider2D _colliderToModify;
    [SerializeField] private PhysicsMaterial2D _bouncyMaterial;
    [SerializeField] private PhysicsMaterial2D _nonBouncyMaterial;

    private TempoDecoder _tempoDecoder;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        _tempoDecoder.OnBeat += ChangeToBouncy;
        _tempoDecoder.OnBeatAfter += ChangeToNonBouncy;
    }

    private void OnDisable() { }

    private void Awake()
    {
        if (!TryGetComponent(out _tempoDecoder))
        {
            Debug.Log(name + ": no tempo decoder found");
        }
    }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void ChangeToNonBouncy()
    {
        _colliderToModify.sharedMaterial = _nonBouncyMaterial;
    }

    private void ChangeToBouncy()
    {
        _colliderToModify.sharedMaterial = _bouncyMaterial;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}