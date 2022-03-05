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
        GameObject player;
        
        if (PhotonNetwork.IsConnected)
        {
            player = PhotonNetwork.Instantiate(_playerData.PlayerPrefab.name,
                _playerData.SpawnPosition, Quaternion.identity);
            player.name = PhotonNetwork.NickName;
        }
        else
        { 
            player = Object.Instantiate(_playerData.PlayerPrefab,
                _playerData.SpawnPosition, Quaternion.identity).gameObject;
            player.name = $"Player {Random.Range(0, 1000)}";
        }

        PlayerView = player.GetComponentInChildren<PlayerView>();
        CharacterController = PlayerView.CharacterController;
        PhotonView = PlayerView.PhotonView;
        GroundCheck = PlayerView.GroundCheck;
        Head = PlayerView.Head;
        SetLayer(player, _playerData.PlayerLayerId);

        return player.gameObject;
    }

    private void SetLayer(GameObject player, int layerId)
    {
        foreach (var child in player.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layerId;
        }
    }
}