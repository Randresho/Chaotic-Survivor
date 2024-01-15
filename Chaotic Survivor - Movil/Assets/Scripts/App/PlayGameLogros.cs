using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.Tilemaps;


public class PlayGameLogros : MonoBehaviour
{
    public static PlayGameLogros instance;
    static bool active = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        if (!active)
        {
            DontDestroyOnLoad(this);
            active = true;
        }
        else
            Destroy(this);

        //Inciar en play games
        try 
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.Authenticate(delegate(SignInStatus status) { });
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool succes) => { });
#endif
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }
    }

    public void SendScore(int score)
    {
#if UNITY_ANDROID
        if(Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkIzoyp2dgGEAIQAQ", success => { });
        }
#endif
    }

    public void SendScoreFreezed(int score)
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkIzoyp2dgGEAIQBg", success => { });
        }
#endif
    }

    public void SendScoreBurn(int score)
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkIzoyp2dgGEAIQBw", success => { });
        }
#endif
    }

    public void SendScoreElectrical(int score)
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkIzoyp2dgGEAIQCA", success => { });
        }
#endif
    }

    public void ShowRanking()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzoyp2dgGEAIQAQ");//First play
        }

        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzoyp2dgGEAIQBg");//Freezed
        }

        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzoyp2dgGEAIQCA");//Electrocuted
        }

        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzoyp2dgGEAIQBw");//Pyromaniac
        }
#endif
    }

    public void WinAchivement()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress("CgkIzoyp2dgGEAIQAg", 100f, succes => { });

        }
#endif
    }

    public void FreezingTime()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress("CgkIzoyp2dgGEAIQAw", 100f, succes => { });

        }
#endif
    }

    public void Electrocuted()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress("CgkIzoyp2dgGEAIQBA", 100f, succes => { });

        }
#endif
    }

    public void Pyromaniac()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress("CgkIzoyp2dgGEAIQBQ", 100f, succes => { });

        }
#endif
    }

    public void ShowAchievements()
    {
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();

        }
#endif
    }
}
