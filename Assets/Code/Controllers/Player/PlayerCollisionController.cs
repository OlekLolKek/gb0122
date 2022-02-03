using System;
using System.Collections;
using UniRx;
using UnityEngine;


public sealed class PlayerCollisionController : IExecutable, ICleanable
{
    private readonly PlayerJumpModel _jumpModel;
    private readonly PlayerView _playerView;
    private readonly LayerMask _groundLayer;
    private readonly float _maxSlopeAngle = 35.0f;
    private readonly float _stopGroundedDelay = 3.0f;
        
    private IDisposable _stopGroundedInvoke;
    private float _deltaTime;
    private bool _isCancellingGrounded;
        

    public PlayerCollisionController(PlayerModel playerModel, PlayerJumpModel jumpModel,
        PlayerData playerData)
    {
        _jumpModel = jumpModel;
        _playerView = playerModel.PlayerView;
        _groundLayer = playerData.GroundLayerMask;
        _playerView.OnCollisionStayEvent += PlayerCollision;
    }
        
    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
    }
        
    private void PlayerCollision(Collision other)
    {
        var layer = other.gameObject.layer;
            
        if (_groundLayer != (_groundLayer | (1 << layer))) return;

        for (var i = 0; i < other.contactCount; i++)
        {
            var normal = other.contacts[i].normal;
            if (IsFloor(normal))
            {
                _jumpModel.IsGrounded = true;
                _isCancellingGrounded = false;
                _jumpModel.NormalVector = normal;
                _stopGroundedInvoke?.Dispose();
            }
        }

        if (!_isCancellingGrounded)
        {
            _isCancellingGrounded = true;
            _stopGroundedInvoke = StopGrounded(_deltaTime * _stopGroundedDelay).ToObservable().Subscribe();
        }
    }
        
    private IEnumerator StopGrounded(float delay)
    {
        yield return new WaitForSeconds(delay);
        _jumpModel.IsGrounded = false;
    }
        
    private bool IsFloor(Vector3 normal)
    {
        var angle = Vector3.Angle(Vector3.up, normal);
        return angle < _maxSlopeAngle;
    }
        
    public void Cleanup()
    {
        _playerView.OnCollisionStayEvent -= PlayerCollision;
    }
}