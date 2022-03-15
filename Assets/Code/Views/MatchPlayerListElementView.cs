using Photon.Pun;
using TMPro;
using UnityEngine;


public sealed class MatchPlayerListElementView : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _deathsText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Color _activeColor;
    
    public void SetTexts(string playerName, int kills, int deaths, int score)
    {
        _playerNameText.text = playerName;
        _killsText.text = kills.ToString();
        _deathsText.text = deaths.ToString();
        _scoreText.text = score.ToString();

        if (playerName == PhotonNetwork.LocalPlayer.NickName)
        {
            _playerNameText.color = _activeColor;
        }
    }
}