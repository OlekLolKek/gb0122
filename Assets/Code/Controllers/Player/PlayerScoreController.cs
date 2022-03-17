using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


public sealed class PlayerScoreController : IMatchStateListener, ICleanable
{
    private readonly ScoreManager _scoreManager;
    private readonly PlayerView _playerView;
    private readonly HudView _hudView;


    public PlayerScoreController(PlayerView playerView, HudView hudView)
    {
        _playerView = playerView;
        _hudView = hudView;

        _scoreManager = Object.FindObjectOfType<ScoreManager>();

        _playerView.OnUpdatedScore += UpdateScore;
    }

    private void UpdateScore(int kills, int deaths, int score)
    {
        _hudView.SetKills(kills);
        _hudView.SetDeaths(deaths);
        _hudView.SetScore(score);
    }

    public void ChangeMatchState(MatchState matchState)
    {
        if (matchState == MatchState.MatchEndCountdown)
        {
            GetAccountInfo();
        }
    }

    private void GetAccountInfo()
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, GotUserData, Debug.LogError);
    }

    private void GotUserData(GetAccountInfoResult result)
    {
        GetUserData();
    }

    private void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = new List<string>
                    { Constants.SCORE_DATA_ID, Constants.TOTAL_SCORE_DATA_ID, Constants.LEVEL_DATA_ID }
            }, result => GotUserScore(result).ToObservable().Subscribe(), Debug.LogError
        );
    }

    private IEnumerator GotUserScore(GetUserDataResult result)
    {
        var data = new PlayerLevelModel();

        if (result.Data != null)
        {
            data.Score = Convert.ToInt32(result.Data[Constants.SCORE_DATA_ID].Value);
            data.TotalScore = Convert.ToInt32(result.Data[Constants.TOTAL_SCORE_DATA_ID].Value);
            data.Level = Convert.ToInt32(result.Data[Constants.LEVEL_DATA_ID].Value);
        }

        Debug.Log($"Got user data: {data.Score} / {data.TotalScore} / {data.Level}");

        var matchScore = _scoreManager.GetStats(PhotonNetwork.LocalPlayer.ActorNumber);
        var currentTotalScore = data.TotalScore;
        var currentScore = data.Score;

        data.TotalScore += matchScore.Score;
        data.Score += matchScore.Score;
        
        var scoreForNextLevel = (int)(5 * Mathf.Pow(data.Level, 2) + 50 * data.Level + 100);
        var totalScoreForNextLevel = CountTotalXpForLevel(data.Level + 1);

        while (currentScore < data.Score)
        {
            ++currentScore;
            ++currentTotalScore;

            if (currentScore >= scoreForNextLevel)
            {
                data.Level++;
                data.Score -= scoreForNextLevel;
                currentScore = 0;

                scoreForNextLevel = (int)(5 * Math.Pow(data.Level, 2) + 50 * data.Level + 100);
                totalScoreForNextLevel = CountTotalXpForLevel(data.Level + 1);
            }
            
            _hudView.SetLevelProgress(currentTotalScore, totalScoreForNextLevel, data.Level,
                (float)currentScore / (float)scoreForNextLevel);
            yield return new WaitForSeconds(0.01f);
        }

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>
                {
                    { Constants.SCORE_DATA_ID, data.Score.ToString() },
                    { Constants.TOTAL_SCORE_DATA_ID, data.TotalScore.ToString() },
                    { Constants.LEVEL_DATA_ID, data.Level.ToString() }
                }
            }, dataResult =>
            {
                Debug.Log($"Updated user data to {data.Score} / {data.TotalScore} / {data.Level}");
            },
            Debug.LogError);
    }

    public int CountTotalXpForLevel(int levelToCount)
    {
        var totalXp = 0;

        for (var i = 0; i < levelToCount; ++i)
        {
            var level = Mathf.Clamp(i, 0, levelToCount + 1);

            totalXp += (int)(5 * Math.Pow(level, 2) + 50 * level + 100);
        }

        return totalXp;
    }

    public void Cleanup()
    {
        _playerView.OnUpdatedScore -= UpdateScore;
    }
}