using UnityEngine;

// Author : Auguste Paccapelo

public class KillZone : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    [SerializeField] private LayerMask _layerThatCanDestroy;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _layerThatCanDestroy) == 0) return;

        //Destroy(collision.gameObject);
        collision.gameObject.SetActive(false);

        GameManager.Instance.RestartGame();

        //if (GameManager.CurrentGameState == GameState.GamePlaying) GameManager.Instance.InitPlayerPlacingPlatform();
    }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}