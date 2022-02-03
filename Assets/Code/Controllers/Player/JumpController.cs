using System.Collections;
using UniRx;
using UnityEngine;


public sealed class JumpController : IExecutable, ICleanable
{
    private readonly PlayerJumpModel _playerJumpModel;
    private readonly LayerMask _groundLayer;
    private readonly Rigidbody _rigidbody;
    private readonly IInputKeyHold _jump;
    
    private readonly float _jumpForce;

    private const float JUMP_NORMAL_MULTIPLIER = 0.5f;
    private const float JUMP_UP_MULTIPLIER = 1.5f;
    private const float JUMP_COOLDOWN = 0.25f;

    private Vector3 _normalVector = Vector3.up;
        
    private bool _isReadyToJump = true;
    private bool _isGrounded;

    public JumpController(PlayerModel playerModel, PlayerData playerData,
        InputModel inputModel, PlayerJumpModel jumpModel)
    {
        _rigidbody = playerModel.Rigidbody;

        _jumpForce = playerData.JumpForce;

        _playerJumpModel = jumpModel;
            
        _jump = inputModel.Jump;
        _jump.OnKeyHeld += IsJumpButtonHeld;
    }
        
    public void Execute(float deltaTime)
    {
        GetValues();
        Jump();
    }

    private void Jump()
    {
        if (!_isReadyToJump || !_playerJumpModel.IsPressingJumpButton || !_isGrounded) 
            return;
            
        _isReadyToJump = false;
        _rigidbody.AddForce(Vector2.up * (_jumpForce * JUMP_UP_MULTIPLIER));
        _rigidbody.AddForce(_normalVector * (_jumpForce * JUMP_NORMAL_MULTIPLIER));

        ResetJump().ToObservable().Subscribe();
    }

    private void GetValues()
    {
        _isGrounded = _playerJumpModel.IsGrounded;
        _normalVector = _playerJumpModel.NormalVector;
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(JUMP_COOLDOWN);
        _isReadyToJump = true;
    }
        
    private void IsJumpButtonHeld(bool isButtonPressed)
    {
        if (isButtonPressed)
        {
            _playerJumpModel.IsPressingJumpButton = true;
        }
        else
        {
            _playerJumpModel.IsPressingJumpButton = false;
        }
    }

    public void Cleanup()
    {
        _jump.OnKeyHeld -= IsJumpButtonHeld;
    }
}