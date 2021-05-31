using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class ArcanoidAuthentication : MonoBehaviour
{


    [HideInInspector] private const string _leaderboard = "CgkIirrEwYEZEAIQAQ";

    public static PlayGamesPlatform platform;

    public UIManager score;



    void Start()
    {
        score = FindObjectOfType<UIManager>();

        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success =>
        {


            if (success)
            {
                Debug.Log("Logged in successfully");
            }
            else
            {
                Debug.Log("Failed to log in");
            }

        });
    }

    private void ScoreUpdate()
    {
        Social.ReportScore(score.Score, _leaderboard, (bool success) => { });
    }

    public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }

    public void ExitFromGPS()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
   
}
