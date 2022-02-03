using UnityEngine;


public sealed class PlayerFactory : IFactory
{
    private readonly PlayerData _playerData;
        
    public Rigidbody Rigidbody { get; private set; }
    public Transform Transform { get; private set; }
    public Transform Head { get; private set; }
    public PlayerView PlayerView { get; private set; }

    public PlayerFactory(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public GameObject Create()
    {
        var player = new GameObject(_playerData.PlayerName);
        Transform = player.transform;
        player.AddComponent<MeshFilter>().mesh = _playerData.PlayerMesh;
        var renderer = player.AddComponent<MeshRenderer>();
        renderer.material = _playerData.PlayerMaterial;
        var collider = player.AddComponent<CapsuleCollider>();
        collider.material = _playerData.PhysicMaterial;
        Transform.localScale = _playerData.PlayerScale;
            
        Rigidbody = player.AddComponent<Rigidbody>();
        Rigidbody.mass = _playerData.Mass;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        Head = new GameObject(_playerData.HeadName).transform;
        Head.parent = player.transform;
        Head.localPosition = _playerData.HeadPosition;

        PlayerView = player.AddComponent<PlayerView>();

        Transform.position = _playerData.SpawnPosition;
        player.layer = _playerData.PlayerLayerID;

        return player;
    }
}