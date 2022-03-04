using System;
using Photon.Pun;
using UnityEngine;


public sealed class PlayerView : MonoBehaviourPunCallbacks, IDamageable, IPunObservable
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private GameObject _head;
        
    [SerializeField] private float _health;

    private Renderer[] _renderers;
    private DamageableUnitsManager _manager;

    private bool _isDead;
    public event Action<float> OnReceivedDamage = delegate {  };

    public CharacterController CharacterController => _characterController;
    public PhotonView PhotonView => _photonView;
    public GameObject GroundCheck => _groundCheck;
    public GameObject Head => _head;
    public GameObject Instance => gameObject;
    public int ID => _photonView.Owner.ActorNumber;

    private void Awake()
    {
        _manager = FindObjectOfType<DamageableUnitsManager>();
        _renderers = GetComponentsInChildren<Renderer>();
        
        if (PhotonNetwork.IsConnected)
        {
            _manager.Register(ID, this);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_health);
            stream.SendNext(_isDead);
        }
        else
        {
            _health = (float)stream.ReceiveNext();
            _isDead = (bool)stream.ReceiveNext();

            SetDead(_isDead);
        }
    }

    public void SetHealth(float health)
    {
        _health = health;
    }
    
    public void Damage(float damage)
    {
        if (_photonView.IsMine)
            OnReceivedDamage.Invoke(damage);
    }

    public void SendIdToDamage(int idToDamage, float damage)
    {
        _photonView.RPC(nameof(RpcSendIdToDamage), RpcTarget.All, idToDamage, damage);
    }
    
    [PunRPC]
    public void RpcSendIdToDamage(int idToDamage, float damage)
    {
        var enemy = _manager.GetDamageable(idToDamage);
        enemy?.Damage(damage);
    }
    
    public bool CheckIfMine()
    {
        return !PhotonNetwork.IsConnected || _photonView.IsMine;
    }
    
    public void SetDead(bool dead)
    {
        _isDead = dead;
        _characterController.enabled = !_isDead;

        foreach (var renderer1 in _renderers)
        {
            renderer1.enabled = !_isDead;
        }
    }
}