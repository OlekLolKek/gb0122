using System.Collections.Generic;
using UnityEngine;


public sealed class ScoreManager : MonoBehaviour
{
    private readonly Dictionary<int, PlayerScoreModel> _playerScores = new Dictionary<int, PlayerScoreModel>();

    private DamageableUnitsManager _unitsManager;
    
    private void Start()
    {
        _unitsManager = FindObjectOfType<DamageableUnitsManager>();
        _unitsManager.OnPlayerListChanged += FetchPlayers;
        
        var players = _unitsManager.GetAllPlayers();

        foreach (var player in players)
        {
            _playerScores.Add(player.ID, new PlayerScoreModel { Nickname = player.Nickname });
        }
    }
    
    public void AddStats(int id, int kills = 0, int deaths = 0, int score = 0)
    {
        if (_playerScores.TryGetValue(id, out var playerScoreModel))
        {
            playerScoreModel.Kills += kills;
            playerScoreModel.Deaths += deaths;
            playerScoreModel.Score += score;
            _playerScores[id] = playerScoreModel;

            _unitsManager.GetDamageable(id)?.SetScore(playerScoreModel.Kills,
                playerScoreModel.Deaths, playerScoreModel.Score);
        }
    }

    public PlayerScoreModel GetStats(int id)
    {
        return _playerScores.ContainsKey(id) ? _playerScores[id] : new PlayerScoreModel();
    }
    
    private void FetchPlayers()
    {
        var players = _unitsManager.GetAllPlayers();

        foreach (var player in players)
        {
            if (!_playerScores.ContainsKey(player.ID))
            {
                _playerScores.Add(player.ID, new PlayerScoreModel { Nickname = player.Nickname });
            }
        }
    }

    private void OnDestroy()
    {
        _unitsManager.OnPlayerListChanged -= FetchPlayers;
    }
}