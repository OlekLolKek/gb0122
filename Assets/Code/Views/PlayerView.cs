using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;


public sealed class PlayerView : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private GameObject _head;

    [SerializeField] private float _health;
    
    public event Action<float> OnReceivedDamage = delegate {  };
    
    public CharacterController CharacterController => _characterController;
    public PhotonView PhotonView => _photonView;
    public GameObject GroundCheck => _groundCheck;
    public GameObject Head => _head;
    public int OwnerActorNumber => _photonView.Owner.ActorNumber;

    private int _playerToDamageActorNumber;
    private float _damageToDeal;

    private const int EMPTY_ACTOR_NUMBER_PACKET = int.MinValue;
    private const float EMPTY_DAMAGE_PACKET = float.MinValue;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        if (stream.IsWriting)
        {
            Debug.Log($"{name} is sending health: {_health}");
            stream.SendNext(_health);
            stream.SendNext(_playerToDamageActorNumber);
            stream.SendNext(_damageToDeal);
            _playerToDamageActorNumber = EMPTY_ACTOR_NUMBER_PACKET;
            _damageToDeal = EMPTY_DAMAGE_PACKET;
        }
        else
        {
            _health = (float)stream.ReceiveNext();
            Debug.Log($"{name} read new health: {_health}");

            _playerToDamageActorNumber = (int)stream.ReceiveNext();
            _damageToDeal = (float)stream.ReceiveNext();

            // TODO: refactor
            if (_playerToDamageActorNumber != EMPTY_ACTOR_NUMBER_PACKET &&
                !Mathf.Approximately(_damageToDeal, EMPTY_DAMAGE_PACKET))
            {
                FindObjectsOfType<PlayerView>()
                    .FirstOrDefault(player => player.OwnerActorNumber == _playerToDamageActorNumber)
                    ?.Damage(_damageToDeal);
            }
        }
    }

    public void SetHealth(float health)
    {
        _health = health;
    }
    
    public void Damage(float damage)
    {
        OnReceivedDamage.Invoke(damage);
    }

    public void SendPlayerToDamage(int playerToDamageActorNumber, float damage)
    {
        _playerToDamageActorNumber = playerToDamageActorNumber;
        _damageToDeal = damage;
    }
}