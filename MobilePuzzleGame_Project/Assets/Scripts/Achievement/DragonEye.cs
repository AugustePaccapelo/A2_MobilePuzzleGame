using UnityEngine;

// Author : Auguste Paccapelo

public class DragonEye : MonoBehaviour, ITouchableOnDown
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

    public void OnTouchedDown(ToucheData data)
    {
        GooglePlayManager.DragonsEyeTouched();
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}