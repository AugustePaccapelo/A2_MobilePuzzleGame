using UnityEngine;

// Author : Auguste Paccapelo

public class CanvaUIObstaclesContainer : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        Canvas.ForceUpdateCanvases();
    }

    private void Start()
    {
        //Canvas canvas = GetComponent<Canvas>();
        //canvas.worldCamera = Camera.main;

    }

    private void Update() { }

    // ----- My Functions ----- \\

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}