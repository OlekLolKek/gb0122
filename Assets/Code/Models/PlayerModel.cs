using UnityEngine;


public sealed class PlayerModel
{
    public CharacterController CharacterController { get; }
    public GameObject GroundCheck { get; }
    public GameObject Head { get; }
    public Transform Transform { get; }


    public bool IsPressingJumpButton { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsGrounded { get; set; }

    public PlayerModel(PlayerFactory factory)
    {
        factory.Create();

        CharacterController = factory.CharacterController;
        GroundCheck = factory.GroundCheck;
        Head = factory.Head;
        Transform = factory.PlayerView.transform;
    }
}