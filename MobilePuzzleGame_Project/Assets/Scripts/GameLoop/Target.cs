using UnityEngine;

// Author : Auguste Paccapelo

public class Target : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    [SerializeField] private LayerMask _layerToDestroy;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1 << go.layer => exact layerMask of go
        // if Note = 6 => 00100000
        if (((1 << collision.gameObject.layer) & _layerToDestroy) == 0) return;

        Destroy(collision.gameObject);
        if (GameManager.CurrentGameState == GameState.GamePlaying)
        {
            Debug.Log("Game Won !");
            GameManager.Instance.FinishLevel();
        }
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}