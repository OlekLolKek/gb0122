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

    public event Action<float, IDamageable> OnReceivedDamage = delegate {  };

    public DamageableUnitsManager DamageableUnitsManager { get; private set; }
    public ScoreManager ScoreManager { get; private set; }
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public WeaponView WeaponView => _weaponView;
    public PhotonView PhotonView => _photonView;
    public GameObject Instance => gameObject;

    public bool IsDead => _isDead;

    public int ID { get; private set; }


    public void Awake()
    {
        DamageableUnitsManager = FindObjectOfType<DamageableUnitsManager>();
        ScoreManager = FindObjectOfType<ScoreManager>();
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetScore(int kills, int deaths, int score) {  }

    public void SetId(int id)
    {
        ID = id;
        DamageableUnitsManager.Register(ID, this);
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

    public void Damage(float damage, IDamageable sender)
    {
        if (_photonView.IsMine)
        {
            OnReceivedDamage.Invoke(damage, sender);
        }
    }

    public void SendIdToDamage(int idToDamage, float damage)
    {
        _photonView.RPC(nameof(RpcSendIdToDamage), RpcTarget.All, idToDamage, damage);
    }

    [PunRPC]
    public void RpcSendIdToDamage(int idToDamage, float damage)
    {
        var enemy = DamageableUnitsManager.GetDamageable(idToDamage);
        enemy?.Damage(damage, this);
    }
    
    [PunRPC]
    public void AddStats(int id, int kills = 0, int deaths = 0, int score = 0)
    {
        ScoreManager.AddStats(id, kills, deaths, score);
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

    private void OnDestroy()
    {
        DamageableUnitsManager.Unregister(ID);
    }
}