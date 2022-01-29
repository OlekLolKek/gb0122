using Photon.Realtime;
using TMPro;
using UnityEngine;


public sealed class PlayersElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;

    public void SetPlayer(Player player)
    {
        _nameText.text = player.UserId;
    }
}