using System;
using Photon.Pun;
using UnityEngine;


public sealed class PlayerModel
{
    public CharacterController CharacterController { get; }
    public PlayerView PlayerView { get; }
    public PhotonView PhotonView { get; }
    public GameObject GroundCheck { get; }
    public GameObject Head { get; }
    public Transform Transform { get; }


    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public bool IsPressingJumpButton { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            DeadChanged.Invoke(_isDead);
        } 
    }

    private bool _isDead;
    
    public event Action<bool> DeadChanged = delegate {  };

    public PlayerModel(PlayerFactory factory)
    {
        factory.Create();

        PlayerView = factory.PlayerView;
        CharacterController = factory.CharacterController;
        PhotonView = factory.PhotonView;
        GroundCheck = factory.GroundCheck;
        Head = factory.Head;
        Transform = PlayerView.transform;
    }
}