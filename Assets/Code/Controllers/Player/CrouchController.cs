using DG.Tweening;
using UnityEngine;


public sealed class CrouchController : IExecutable, ICleanable
{
    private readonly IInputKeyPress _startCrouch;
    private readonly IInputKeyRelease _stopCrouch;
        
    private readonly PlayerJumpModel _playerJumpModel;
    private readonly PlayerCrouchModel _playerCrouchModel;
    private readonly PlayerModel _playerModel;
    private readonly Transform _transform;
    private readonly Rigidbody _rigidbody;
    private readonly Vector3 _crouchScale;
    private readonly Vector3 _playerScale;

    private readonly float _crouchTime = 0.05f;
    private readonly float _crouchBoostSpeed = 1.0f;
    private readonly float _crouchHeight = 0.75f;
    private readonly float _slideForce = 750;


    public CrouchController(InputModel inputModel, PlayerCrouchModel crouchModel,
        PlayerJumpModel jumpModel, PlayerModel playerModel,
        PlayerData playerData)
    {
        _startCrouch = inputModel.StartCrouch;
        _stopCrouch = inputModel.StopCrouch;
        _startCrouch.OnKeyPressed += StartCrouch;
        _stopCrouch.OnKeyReleased += StopCrouch;

        _playerCrouchModel = crouchModel;

        _playerJumpModel = jumpModel;

        _playerModel = playerModel;
        _transform = _playerModel.Transform;
        _rigidbody = _playerModel.Rigidbody;
            
        _crouchScale = playerData.CrouchScale;
        _playerScale = playerData.PlayerScale;
    }
        
    public void Execute(float deltaTime)
    {
    }
        
    private void StartCrouch()
    {
        _playerCrouchModel.IsCrouching = true;
        _transform.DOScale(_crouchScale, _crouchTime);
        var position = _transform.position;
        _transform.DOMoveY(position.y - _crouchHeight, _crouchTime);
            
        if (_playerJumpModel.IsGrounded)
        {
            if (_rigidbody.velocity.magnitude > _crouchBoostSpeed)
            {
                var velocity = _rigidbody.velocity.normalized;
                var direction = new Vector3(velocity.x, 0.0f, velocity.z);
                _rigidbody.AddForce(direction * _slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        _playerCrouchModel.IsCrouching = false;
        _transform.DOScale(_playerScale, _crouchTime);
        var position = _transform.position;
        _transform.DOMoveY(position.y + _crouchHeight, _crouchTime);
    }

    public void Cleanup()
    {
        _startCrouch.OnKeyPressed -= StartCrouch;
        _stopCrouch.OnKeyReleased -= StopCrouch;
    }
}