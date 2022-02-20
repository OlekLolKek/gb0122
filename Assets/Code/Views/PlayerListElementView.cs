using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class PlayerListElementView : MonoBehaviour
{
    [SerializeField] private TMP_Text _usernameText;
    [SerializeField] private Button _kickButton;

    private Player _player;
    public int PlayerActorNumber { get; private set; }


    public event Action<Player> OnKickButtonClicked = delegate {  };

    private void Start()
    {
        _kickButton.onClick.AddListener(KickButtonClicked);
    }

    private void OnDestroy()
    {
        _kickButton.onClick.RemoveListener(KickButtonClicked);
    }

    private void KickButtonClicked()
    {
        OnKickButtonClicked.Invoke(_player);
    }

    public void Initialize(Player player, string playerUsername)
    {
        _player = player;
        PlayerActorNumber = _player.ActorNumber;

        _usernameText.text = playerUsername;
        
        if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerActorNumber || !PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _kickButton.gameObject.SetActive(false);
        }
    }
}