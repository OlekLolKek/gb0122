using UnityEngine;


public sealed class PlayerJumpModel
{
    public bool IsGrounded { get; set; }
    public bool IsPressingJumpButton { get; set; }
    public Vector3 NormalVector { get; set; }
}