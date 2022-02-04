using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public sealed class PlayerData : ScriptableObject, IData
{
    [Header("Prefab")]
    [SerializeField] private PlayerView _playerPrefab;
    [SerializeField] private bool _usePrefab;

    [Header("Factory parameters")]
    [SerializeField] private PhysicMaterial _physicMaterial;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Material _playerMaterial;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private Vector3 _playerScale;
    [SerializeField] private Vector3 _crouchScale;
    [SerializeField] private Vector3 _headPosition;
    [SerializeField] private Mesh _playerMesh;
    [SerializeField] private string _playerName;
    [SerializeField] private string _headName;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _mass;
    [SerializeField] private int _playerLayerID;


    public PlayerView PlayerPrefab => _playerPrefab;
    public bool UsePrefab => _usePrefab;
    
    public PhysicMaterial PhysicMaterial => _physicMaterial;
    public LayerMask GroundLayerMask => _groundLayerMask;
    public Material PlayerMaterial => _playerMaterial;
    public Vector3 SpawnPosition => _spawnPosition;
    public Vector3 PlayerScale => _playerScale;
    public Vector3 CrouchScale => _crouchScale;
    public Vector3 HeadPosition => _headPosition;
    public Mesh PlayerMesh => _playerMesh;
    public string PlayerName => _playerName;
    public string HeadName => _headName;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float Mass => _mass;
    public int PlayerLayerID => _playerLayerID;
}