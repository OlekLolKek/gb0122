﻿using System;
using System.Linq;
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


    private float _damageToDeal;
    private bool _isDead;
    private int _enemyToDamageId;

    private const int EMPTY_ACTOR_NUMBER_PACKET = int.MaxValue;
    private const float EMPTY_DAMAGE_PACKET = float.MaxValue;
    public event Action<float> OnReceivedDamage = delegate {  };

    public CharacterController CharacterController => _characterController;
    public PhotonView PhotonView => _photonView;
    public GameObject GroundCheck => _groundCheck;
    public GameObject Head => _head;
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
            
            stream.SendNext(_enemyToDamageId);
            stream.SendNext(_damageToDeal);
            _enemyToDamageId = EMPTY_ACTOR_NUMBER_PACKET;
            _damageToDeal = EMPTY_DAMAGE_PACKET;
        }
        else
        {
            _health = (float)stream.ReceiveNext();
            _isDead = (bool)stream.ReceiveNext();

            _enemyToDamageId = (int)stream.ReceiveNext();
            _damageToDeal = (float)stream.ReceiveNext();
            
            if (_enemyToDamageId != EMPTY_ACTOR_NUMBER_PACKET &&
                !Mathf.Approximately(_damageToDeal, EMPTY_DAMAGE_PACKET))
            {
                var enemy = _manager.GetDamageable(_enemyToDamageId);
                enemy?.Damage(_damageToDeal);
            }
            
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
        _enemyToDamageId = idToDamage;
        _damageToDeal = damage;
        
        Debug.Log($"Sending {damage} to {idToDamage} from {name}");
    }
    
    public bool CheckIfMine()
    {
        return !PhotonNetwork.IsConnected || _photonView.IsMine;
    }
    
    public void SetDead(bool dead)
    {
        _isDead = dead;
        _characterController.enableOverlapRecovery = !_isDead;
        
        Debug.Log($"Setting renderers to {!dead}");
        // VOT TUT CHECK POCHEMU RENDERERS SET ONLY TO TRUE AND NOT FALSE
        
        foreach (var renderer1 in _renderers)
        {
            renderer1.enabled = !_isDead;
        }
    }
}