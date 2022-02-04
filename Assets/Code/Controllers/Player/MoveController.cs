using System;
using UnityEngine;


public sealed class MoveController : IExecutable, ICleanable
{
    private readonly PlayerJumpModel _playerJumpModel;
    private readonly PlayerCrouchModel _playerCrouchModel;
        
    private readonly Transform _transform;
    private readonly Rigidbody _rigidbody;
    private readonly float _moveSpeed;
    private readonly float _maxSpeed = 20.0f;
    private readonly float _counterMovement = 0.175f;
    private readonly float _counterMovementThreshold = 0.01f;
    private readonly float _axisThreshold = 0.05f;
    private readonly float _extraGravity = 300.0f;
    private readonly float _crouchingGravity = 3000;
        
    private float _deltaTime;
    private float _horizontal;
    private float _vertical;
    private bool _isPressingJumpButton;
    private bool _isReadyToJump = true;
    private bool _isCrouching;
    private bool _isGrounded;

    private readonly IInputAxisChange _horizontalAxis;
    private readonly IInputAxisChange _verticalAxis;

    //TODO: eto che
    private IDisposable _stopGroundedInvoke;


    public MoveController(PlayerModel playerModel, PlayerData playerData, 
        InputModel inputModel, PlayerJumpModel jumpModel,
        PlayerCrouchModel crouchModel)
    {
        _rigidbody = playerModel.Rigidbody;
        _transform = playerModel.Transform;
            
        _moveSpeed = playerData.Speed;

        _horizontalAxis = inputModel.Horizontal;
        _verticalAxis = inputModel.Vertical;
        _horizontalAxis.OnAxisChanged += HorizontalAxisChanged;
        _verticalAxis.OnAxisChanged += VerticalAxisChanged;

        _playerJumpModel = jumpModel;
        _playerCrouchModel = crouchModel;
    }
        
    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
        GetValues();
        Move();
    }

    private void GetValues()
    {
        _isGrounded = _playerJumpModel.IsGrounded;
        _isPressingJumpButton = _playerJumpModel.IsPressingJumpButton;

        _isCrouching = _playerCrouchModel.IsCrouching;
    }

    private void Move()
    {
        _rigidbody.AddForce(Vector3.down * (_deltaTime * _extraGravity));
            
        var magnitude = FindVelocityRelativeToLook();

        CounterMovement(magnitude, _deltaTime);

        if (!_isCrouching || !_isGrounded || !_isReadyToJump)
        {
            var multiplier = 1.0f;
            var multiplierForward = 1.0f;

            if (!_isGrounded)
            {
                multiplier = 0.5f;
                multiplierForward = 0.5f;
            }

            _rigidbody.AddForce(_transform.forward *
                                (_vertical * _moveSpeed * _deltaTime * multiplier * multiplierForward));
            _rigidbody.AddForce(_transform.right * (_horizontal * _moveSpeed * _deltaTime * multiplier));
        }
        else
        {
            _rigidbody.AddForce(Vector3.down * (_deltaTime * _crouchingGravity));
            return;
        }
    }
    
    private void CounterMovement(Vector2 magnitude, float deltaTime)
    {
        if (!_isGrounded)
            return;
        if (_isPressingJumpButton)
            return;
            
        if (Math.Abs(magnitude.x) > _counterMovementThreshold && Math.Abs(_horizontal) < _axisThreshold ||
            (magnitude.x < -_counterMovementThreshold && _horizontal > 0) ||
            magnitude.x > _counterMovementThreshold && _horizontal < 0)
        {
            _rigidbody.AddForce(_transform.right * (_moveSpeed * deltaTime * -magnitude.x * _counterMovement));
        }

        if (Math.Abs(magnitude.y) > _counterMovementThreshold && Math.Abs(_vertical) < _axisThreshold ||
            (magnitude.y < -_counterMovementThreshold && _vertical > 0) ||
            (magnitude.y > _counterMovementThreshold && _vertical < 0))
        {
            _rigidbody.AddForce(_transform.forward * (_moveSpeed * deltaTime * -magnitude.y * _counterMovement));
        }

        if (Mathf.Sqrt((Mathf.Pow(_rigidbody.velocity.x, 2) + Mathf.Pow(_rigidbody.velocity.z, 2))) > _maxSpeed)
        {
            var velocity = _rigidbody.velocity;
            var fallSpeed = velocity.y;
            var newVelocity = velocity.normalized * _maxSpeed;
            velocity = new Vector3(newVelocity.x, fallSpeed, newVelocity.z);
            _rigidbody.velocity = velocity;
        }
    }

    private Vector2 FindVelocityRelativeToLook()
    {
        var lookAngle = _transform.eulerAngles.y;
        var velocity = _rigidbody.velocity;
        var moveAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

        var deltaAngleY = Mathf.DeltaAngle(lookAngle, moveAngle);
        var deltaAngleX = 90 - deltaAngleY;
        var magnitude = _rigidbody.velocity.magnitude;
        var magnitudeY = magnitude * Mathf.Cos(deltaAngleY * Mathf.Deg2Rad);
        var magnitudeX = magnitude * Mathf.Cos(deltaAngleX * Mathf.Deg2Rad);

        return new Vector2(magnitudeX, magnitudeY);
    }

    private void HorizontalAxisChanged(float value)
    {
        _horizontal = value;
    }
        
    private void VerticalAxisChanged(float value)
    {
        _vertical = value;
    }

    public void Cleanup()
    {
        _horizontalAxis.OnAxisChanged -= HorizontalAxisChanged;
        _verticalAxis.OnAxisChanged -= VerticalAxisChanged;
    }
}