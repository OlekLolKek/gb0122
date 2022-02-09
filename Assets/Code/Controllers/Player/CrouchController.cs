using DG.Tweening;
using UnityEngine;


public sealed class CrouchController : ICleanable
{
    private readonly IInputKeyPress _startCrouch;
    private readonly IInputKeyRelease _stopCrouch;
    
    private readonly PlayerModel _playerModel;
    private readonly Transform _transform;
    private readonly Vector3 _crouchScale;
    private readonly Vector3 _playerScale;

    private const float CROUCH_TIME = 0.05f;
    private const float CROUCH_HEIGHT = 0.6f;


    public CrouchController(InputModel inputModel, PlayerModel playerModel,
        PlayerData playerData)
    {
        _startCrouch = inputModel.StartCrouch;
        _stopCrouch = inputModel.StopCrouch;
        _startCrouch.OnKeyPressed += StartCrouch;
        _stopCrouch.OnKeyReleased += StopCrouch;

        _playerModel = playerModel;
        _transform = _playerModel.Transform;

        _crouchScale = playerData.CrouchScale;
        _playerScale = playerData.PlayerScale;
    }

    private void StartCrouch()
    {
        _playerModel.IsCrouching = true;
        _transform.DOScale(_crouchScale, CROUCH_TIME);
        var position = _transform.position;
        _transform.DOMoveY(position.y - CROUCH_HEIGHT, CROUCH_TIME);
    }

    private void StopCrouch()
    {
        _playerModel.IsCrouching = false;
        _transform.DOScale(_playerScale, CROUCH_TIME);
        var position = _transform.position;
        _transform.DOMoveY(position.y + CROUCH_HEIGHT, CROUCH_TIME);
    }

    public void Cleanup()
    {
        _startCrouch.OnKeyPressed -= StartCrouch;
        _stopCrouch.OnKeyReleased -= StopCrouch;
    }
}