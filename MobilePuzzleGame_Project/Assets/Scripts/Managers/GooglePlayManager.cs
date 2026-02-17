using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

// Author : Auguste Paccapelo

public class GooglePlayManager : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Singleton ----- \\

    public static GooglePlayManager Instance {get; private set;}

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable() { }

    private void OnDisable() { }

    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Debug.Log(nameof(GooglePlayManager) + " Instance already exist, destorying last added.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Google play (je crois)
        //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        // Unity
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    void Update() { }

    // ----- My Functions ----- \\

    internal void ProcessAuthentication(bool status)
    {
        if (status)
        {
            Debug.Log("Singed in!");
        }
        else
        {
            Debug.Log("Not signed in.");
        }
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Singed in!");
        }
        else
        {
            Debug.Log("Not signed in.");
        }
    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}