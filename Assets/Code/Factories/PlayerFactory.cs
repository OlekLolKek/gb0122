using Photon.Pun;
using UnityEngine;


public sealed class PlayerFactory : IFactory
{
    private readonly PlayerSpawnPointView[] _spawnPoints;
    private readonly PlayerData _playerData;

    public CharacterController CharacterController { get; private set; }
    public PlayerView PlayerView { get; private set; }
    public PhotonView PhotonView { get; private set; }
    public GameObject GroundCheck { get; private set; }
    public GameObject Head { get; private set; }

    public PlayerFactory(PlayerData playerData, PlayerSpawnPointView[] spawnPoints)
    {
        _playerData = playerData;
        _spawnPoints = spawnPoints;
    }

    public GameObject Create()
    {
        GameObject player;

        var (position, rotation) = GetRandomPosition();
        
        if (PhotonNetwork.IsConnected)
        {
            player = PhotonNetwork.Instantiate(_playerData.PlayerPrefab.name,
                position, rotation);
            player.name = PhotonNetwork.NickName;
        }
        else
        { 
            player = Object.Instantiate(_playerData.PlayerPrefab,
                position, rotation).gameObject;
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
    
    private (Vector3, Quaternion) GetRandomPosition()
    {
        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

        var xOffset = Random.Range(-2, 2);
        var zOffset = Random.Range(-2, 2);

        var position = spawnPoint.position;
        position.x += xOffset;
        position.z += zOffset;

        return (position, spawnPoint.rotation);
    }

    private void SetLayer(GameObject player, int layerId)
    {
        foreach (var child in player.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layerId;
        }
    }
}