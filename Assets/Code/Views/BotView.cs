using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;


public sealed class BotView : MonoBehaviour, IDamageable, IPunObservable
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private WeaponView _weaponView;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Collider _collider;
    [SerializeField] private float _health;

    private Renderer[] _renderers;
    private bool _isDead;

    public event Action<float> OnReceivedDamage = delegate {  };

    public DamageableUnitsManager Manager { get; private set; }
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public WeaponView WeaponView => _weaponView;
    public GameObject Instance => gameObject;
    public bool IsDead => _isDead;
    public int ID { get; private set; }

    public void Awake()
    {
        Manager = FindObjectOfType<DamageableUnitsManager>();
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_health);
            stream.SendNext(ID);
            stream.SendNext(_isDead);
        }
        else
        {
            _health = (float)stream.ReceiveNext();
            ID = (int)stream.ReceiveNext();
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
        {
            OnReceivedDamage.Invoke(damage);
        }
    }

    public void SendIdToDamage(int idToDamage, float damage)
    {
        _photonView.RPC(nameof(RpcSendIdToDamage), RpcTarget.All, idToDamage, damage);
    }
    
    [PunRPC]
    public void RpcSendIdToDamage(int idToDamage, float damage)
    {
        var enemy = Manager.GetDamageable(idToDamage);
        enemy?.Damage(damage);
    }

    public void SetId(int id)
    {
        ID = id;
        Manager.Register(ID, this);
    }

    public bool CheckIfMine()
    {
        return _photonView.IsMine;
    }

    public void SetDead(bool dead)
    {
        _isDead = dead;
        _navMeshAgent.enabled = !_isDead;
        _collider.enabled = !_isDead;
        
        foreach (var renderer1 in _renderers)
        {
            renderer1.enabled = !_isDead;
        }
    }
}