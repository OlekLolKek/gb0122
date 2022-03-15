using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;


public sealed class HudView : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private DamageableUnitsManager _damageableUnitsManager;
    [SerializeField] private ScoreManager _scoreManager;
    
    [Header("HUD")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _deathsText;
    [SerializeField] private TMP_Text _scoreText;
    
    [Header("Death panel")]
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private TMP_Text _deadText;
    [SerializeField] private TMP_Text _deathTimerText;

    [Header("Start countdown")]
    [SerializeField] private GameObject _startCountdownPanel;
    [SerializeField] private TMP_Text _startCountdownText;

    [Header("End countdown")]
    [SerializeField] private MatchPlayerListElementView _matchPlayerListElementPrefab;
    [SerializeField] private GameObject _endCountdownPanel;
    [SerializeField] private Transform _playerElementsRoot;
    [SerializeField] private TMP_Text _endCountdownText;
    
    [Header("Strings")]
    [SerializeField] private string _basicHealthText;
    [SerializeField] private string _basicEndTimerText;

    private List<MatchPlayerListElementView> _playerListElements = new List<MatchPlayerListElementView>();

    public void SetHealth(float health)
    {
        _healthText.text = _basicHealthText + health;
    }

    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        _ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    public void SetKills(int kills)
    {
        _killsText.text = kills.ToString();
    }

    public void SetDeaths(int deaths)
    {
        _deathsText.text = deaths.ToString();
    }

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void SetDead(bool dead, string deathText)
    {
        _deathPanel.SetActive(dead);
        _deadText.text = deathText;
    }

    public void SetStartCountdown(bool active, float countdown)
    {
        if (countdown < 0.0f) 
            countdown = 0.0f;
        
        _startCountdownPanel.gameObject.SetActive(active);
        _startCountdownText.text = $"{countdown:F2}";
    }
    
    public void SetEndCountdown(bool active, float countdown)
    {
        if (countdown < 0.0f) 
            countdown = 0.0f;
        
        _endCountdownPanel.gameObject.SetActive(active);
        _endCountdownText.text = $"{_basicEndTimerText}{countdown:F2}";

        if (active && _playerListElements.Count == 0)
        {
            var players = _damageableUnitsManager.GetAllPlayers();

            foreach (var player in players)
            {
                var score = _scoreManager.GetStats(player.ID);
                var element = Instantiate(_matchPlayerListElementPrefab, _playerElementsRoot);
                
                element.SetTexts(score.Nickname, score.Kills, score.Deaths, score.Score);

                _playerListElements.Add(element);
            }
        }
    }

    public void SetTimer(float timer)
    {
        if (timer < 0.0f) 
            timer = 0.0f;
        
        _deathTimerText.text = $"{timer:F2}";
    }
}