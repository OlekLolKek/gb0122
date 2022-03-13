using System.Collections.Generic;
using UnityEngine;


public sealed class ScoreManager : MonoBehaviour
{
    private readonly Dictionary<int, PlayerScoreModel> _playerScores = new Dictionary<int, PlayerScoreModel>();

    private DamageableUnitsManager _unitsManager;
    
    private void Start()
    {
        _unitsManager = FindObjectOfType<DamageableUnitsManager>();
        _unitsManager.OnPlayerListChanged += GetPlayers;
        
        var players = _unitsManager.GetAllPlayers();

        foreach (var player in players)
        {
            _playerScores.Add(player.ID, new PlayerScoreModel());
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
    
    private void GetPlayers()
    {
        var players = _unitsManager.GetAllPlayers();

        foreach (var player in players)
        {
            if (!_playerScores.ContainsKey(player.ID))
            {
                _playerScores.Add(player.ID, new PlayerScoreModel());
            }
        }
    }

    private void OnDestroy()
    {
        _unitsManager.OnPlayerListChanged -= GetPlayers;
    }
}