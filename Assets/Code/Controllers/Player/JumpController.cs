using UnityEngine;


public sealed class JumpController : IExecutable, ICleanable
{
    private readonly CharacterController _characterController;
    private readonly PlayerModel _playerModel;
    private readonly LayerMask _groundMask;
    private readonly IInputKeyHold _jump;

    private readonly float _groundCheckRadius;
    private readonly float _jumpForce;

    private const float GROUNDED_GRAVITY = -2.0f;
    private const float GRAVITY = -9.81f;
    
    private Vector3 _velocity;

    public JumpController(PlayerModel playerModel, PlayerData playerData, InputModel inputModel)
    {
        _playerModel = playerModel;
        _characterController = _playerModel.CharacterController;

        _groundMask = playerData.GroundLayerMask;
        _groundCheckRadius = playerData.GroundCheckRadius;
        _jumpForce = playerData.JumpForce;

        _jump = inputModel.Jump;
        _jump.OnKeyHeld += IsJumpButtonHeld;
    }

    public void Execute(float deltaTime)
    {
        GetValues();
        Jump(deltaTime);
    }

    private void Jump(float deltaTime)
    {
        if (_playerModel.IsGrounded && _velocity.y < 0)
        {
            _velocity.y = GROUNDED_GRAVITY;

            if (_playerModel.IsPressingJumpButton)
            {
                _velocity.y = Mathf.Sqrt(_jumpForce * -2.0f * GRAVITY);
            }
        }
        else
        {
            _velocity.y -= GRAVITY * -2.0f * deltaTime;
        }

        _characterController.Move(_velocity * deltaTime);
    }

    private void GetValues()
    {
        _playerModel.IsGrounded = Physics.CheckSphere(_playerModel.GroundCheck.transform.position,
            _groundCheckRadius, _groundMask);
    }

    private void IsJumpButtonHeld(bool isButtonPressed)
    {
        _playerModel.IsPressingJumpButton = isButtonPressed;
    }

    public void Cleanup()
    {
        _jump.OnKeyHeld -= IsJumpButtonHeld;
    }
}