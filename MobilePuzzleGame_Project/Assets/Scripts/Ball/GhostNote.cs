using UnityEngine;

// Author : Auguste Paccapelo

public class GhostNote : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    [SerializeField] private float _lifeDuration = 4f;
    private float _timer = 0f;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _lifeDuration)
        {
            Destroy(gameObject);
        }
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}