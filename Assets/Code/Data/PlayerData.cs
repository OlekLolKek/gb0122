using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public sealed class PlayerData : ScriptableObject, IData
{
    [Header("Prefab")]
    [SerializeField] private PlayerView _playerPrefab;

    [Header("Parameters")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private Vector3 _playerScale;
    [SerializeField] private Vector3 _crouchScale;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckRadius;


    public PlayerView PlayerPrefab => _playerPrefab;
    public LayerMask GroundLayerMask => _groundLayerMask;
    public Vector3 SpawnPosition => _spawnPosition;
    public Vector3 PlayerScale => _playerScale;
    public Vector3 CrouchScale => _crouchScale;
    public float CrouchSpeed => _crouchSpeed;
    public float MoveSpeed => _moveSpeed;
    public float JumpForce => _jumpForce;
    public float GroundCheckRadius => _groundCheckRadius;
}