using UnityEngine;


public sealed class PlayerModel
{
    public Rigidbody Rigidbody { get; }
    public Transform Transform { get; }
    public Transform Head { get; }
    public PlayerView PlayerView { get; }

    public PlayerModel(PlayerFactory factory)
    {
        factory.Create();
        Rigidbody = factory.Rigidbody;
        Transform = factory.Transform;
        Head = factory.Head;
        PlayerView = factory.PlayerView;
    }
}