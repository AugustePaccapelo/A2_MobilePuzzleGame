using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

// Author : Auguste Paccapelo

public enum AchivementEnum
{
    AieMonOeuil = 0,
    MaisArrete = 1,
    DouxBruit = 2,
    SacreRythme = 3,
    Teleportation = 4,
    CaSonne = 5,
    Puissance = 6,
    VoieDouble = 7,
    DansLeTemps = 8,
    CaSouffle = 9
}

public class GooglePlayManager : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Singleton ----- \\

    public static GooglePlayManager Instance {get; private set;}

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    private static Dictionary<AchivementEnum, string> _mapAchievmentIds = new()
    {
        {AchivementEnum.AieMonOeuil, "CgkI0qDllKYWEAIQAQ" },
        {AchivementEnum.MaisArrete, "CgkI0qDllKYWEAIQAg" },
        {AchivementEnum.DouxBruit, "CgkI0qDllKYWEAIQAw" },
        {AchivementEnum.SacreRythme, "CgkI0qDllKYWEAIQBA" },
        {AchivementEnum.Teleportation, "CgkI0qDllKYWEAIQBQ" },
        {AchivementEnum.CaSonne, "CgkI0qDllKYWEAIQBg" },
        {AchivementEnum.Puissance, "CgkI0qDllKYWEAIQBw" },
        {AchivementEnum.VoieDouble, "CgkI0qDllKYWEAIQCA" },
        {AchivementEnum.DansLeTemps, "CgkI0qDllKYWEAIQCQ" },
        {AchivementEnum.CaSouffle, "CgkI0qDllKYWEAIQCg" }
    };

    private static Dictionary<string, bool> _mapAchivementsState = new();

    private static bool _isLoged = true;

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
#if UNITY_EDITOR
        _isLoged = false;
#else
            // Enfaite google play fait tous pour nous pas besoin de code
            // Par contre jsp comment tu peux confirmer que tu est log

            // Google play (je crois)
            //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            //if (!_isLoged)
            //{
            //    Application.Quit();
            //} 
            // Unity
            //PlayGamesPlatform.Activate();
            //Social.localUser.Authenticate(ProcessAuthentication);
    
            _isLoged = true;

            Social.LoadAchievements(LoadAchievements);
#endif
    }

    void Update() { }

    // ----- My Functions ----- \\

    static public void ObstaclePlaced(PlacableObstacle obstacle)
    {
        switch (obstacle)
        {
            case PlacableObstacle.Wall:
                CompleteAchievement(AchivementEnum.DouxBruit);
                break;

            case PlacableObstacle.Drum:
                CompleteAchievement(AchivementEnum.SacreRythme);
                break;
            case PlacableObstacle.Portal:
                CompleteAchievement(AchivementEnum.Teleportation);
                break;
            case PlacableObstacle.Cor:
                CompleteAchievement(AchivementEnum.CaSonne);
                break;
            case PlacableObstacle.Accordeon:
                CompleteAchievement(AchivementEnum.Puissance);
                break;
            case PlacableObstacle.Trumpet:
                CompleteAchievement(AchivementEnum.CaSouffle);
                break;
        }
    }

    static public void CompleteAchievement(AchivementEnum achievement)
    {
        if (!_isLoged) return;
        if (!_mapAchievmentIds.ContainsKey(achievement)) return;

        if (!IsAchievementFinished(achievement))
        {
            Social.ReportProgress(_mapAchievmentIds[achievement], 100.0f, (bool success) => { });
        }
    }

    static public void DragonsEyeTouched()
    {
        if (!_isLoged) return;

        CompleteAchievement(AchivementEnum.AieMonOeuil);

        if (!IsAchievementFinished(AchivementEnum.MaisArrete))
        {
            // PlayGamesPlatform.Instance.IncrementAchievement(_mapAchievmentIds[AchivementEnum.MaisArrete], 1, (bool success) => { });
        }
    }

    private static void LoadAchievements(IAchievement[] achievements)
    {
        if (!_isLoged) return;

        foreach (IAchievement achievement in achievements)
        {
            Social.ReportProgress(achievement.id, 0.0f, (bool _) => { });
            _mapAchivementsState.Add(achievement.id, achievement.completed);
        }
    }

    private static bool IsAchievementFinished(AchivementEnum achievement)
    {
        if (!_isLoged) return false;
        if (!_mapAchievmentIds.ContainsKey(achievement)) return false;
        if (!_mapAchivementsState.ContainsKey(_mapAchievmentIds[achievement])) return false;

        return _mapAchivementsState[_mapAchievmentIds[achievement]];
    }

    internal void ProcessAuthentication(bool status)
    {
        _isLoged = status;

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
            _isLoged = true;
        }
        else
        {
            Debug.Log("Not signed in.");
            _isLoged = false;
        }
    }

    // ----- Destructor ----- \\

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}