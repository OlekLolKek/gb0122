using Photon.Pun;
using UnityEngine;


public sealed class PlayerFactory : IFactory
{
    private readonly PlayerData _playerData;

    public CharacterController CharacterController { get; private set; }
    public PlayerView PlayerView { get; private set; }
    public PhotonView PhotonView { get; private set; }
    public GameObject GroundCheck { get; private set; }
    public GameObject Head { get; private set; }

    public PlayerFactory(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public GameObject Create()
    {
        var player = PhotonNetwork.Instantiate(_playerData.PlayerPrefab.name,
            _playerData.SpawnPosition, Quaternion.identity);

        PlayerView = player.GetComponentInChildren<PlayerView>();
        CharacterController = PlayerView.CharacterController;
        PhotonView = PlayerView.PhotonView;
        GroundCheck = PlayerView.GroundCheck;
        Head = PlayerView.Head;

        return player.gameObject;
    }
}