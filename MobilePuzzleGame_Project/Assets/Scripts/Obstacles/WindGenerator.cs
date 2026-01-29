using UnityEngine;

// Author : Auguste Paccapelo

public class WindGenerator : MonoBehaviour, IInteractible
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    public void TriggerAction()
    {
        Debug.Log("Wind generator triggered");
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}