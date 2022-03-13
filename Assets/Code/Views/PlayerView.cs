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

    private DamageableUnitsManager _damageableUnitsManager;

    private Renderer[] _renderers;

    private bool _isDead;
    public event Action<float, IDamageable> OnReceivedDamage = delegate {  };
    public event Action<int, int, int> OnUpdatedScore = delegate {  };

    public CharacterController CharacterController => _characterController;
    public ScoreManager ScoreManager { get; private set; }
    public PhotonView PhotonView => _photonView;
    public GameObject GroundCheck => _groundCheck;
    public GameObject Head => _head;
    public GameObject Instance => gameObject;
    public bool IsDead => _isDead;
    public int ID => _photonView.Owner.ActorNumber;

    private void Awake()
    {
        _damageableUnitsManager = FindObjectOfType<DamageableUnitsManager>();
        ScoreManager = FindObjectOfType<ScoreManager>();
        
        _renderers = GetComponentsInChildren<Renderer>();
        
        if (PhotonNetwork.IsConnected)
        {
            _damageableUnitsManager.Register(ID, this);
        }

        OnUpdatedScore.Invoke(0, 0, 0);
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

    public void SetScore(int kills, int deaths, int score)
    {
        OnUpdatedScore.Invoke(kills, deaths, score);
    }
    
    public void Damage(float damage, IDamageable sender)
    {
        Debug.Log($"Trying to damage player {name}");
        
        if (_photonView.IsMine)
            OnReceivedDamage.Invoke(damage, sender);
    }

    public void SendIdToDamage(int idToDamage, float damage)
    {
        _photonView.RPC(nameof(RpcSendIdToDamage), RpcTarget.All, idToDamage, damage);
    }
    
    [PunRPC]
    public void RpcSendIdToDamage(int idToDamage, float damage)
    {
        var enemy = _damageableUnitsManager.GetDamageable(idToDamage);
        enemy?.Damage(damage, this);
    }
    
    [PunRPC]
    public void AddStats(int id, int kills = 0, int deaths = 0, int score = 0)
    {
        ScoreManager.AddStats(id, kills, deaths, score);
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