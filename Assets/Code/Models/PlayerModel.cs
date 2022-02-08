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


    public bool IsPressingJumpButton { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsGrounded { get; set; }

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