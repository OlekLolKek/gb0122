using UnityEngine;


public sealed class MoveController : IExecutable, IMatchStateListener, ICleanable
{
    private readonly CharacterController _characterController;
    private readonly PlayerModel _playerModel;
    
    private readonly IInputAxisChange _horizontalAxis;
    private readonly IInputAxisChange _verticalAxis;

    private readonly float _crouchSpeed;
    private readonly float _moveSpeed;

    private float _deltaTime;
    private float _horizontal;
    private float _vertical;

    private MatchState _matchState;

    private const float MAX_VECTOR_LENGTH = 1.0f;
    
    public MoveController(PlayerModel playerModel, PlayerData playerData, 
        InputModel inputModel)
    {
        _playerModel = playerModel;
        
        _characterController = _playerModel.CharacterController;

        _crouchSpeed = playerData.CrouchSpeed;
        _moveSpeed = playerData.MoveSpeed;

        _horizontalAxis = inputModel.Horizontal;
        _verticalAxis = inputModel.Vertical;
        _horizontalAxis.OnAxisChanged += HorizontalAxisChanged;
        _verticalAxis.OnAxisChanged += VerticalAxisChanged;
    }
    
    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
        Move();
    }

    private void Move()
    {
        if (_matchState != MatchState.MatchProcess)
        {
            return;
        }
        
        var moveVector = _playerModel.Transform.right * _horizontal + _playerModel.Transform.forward * _vertical;
        moveVector = Vector3.ClampMagnitude(moveVector, MAX_VECTOR_LENGTH);
        var speed = _playerModel.IsCrouching && _playerModel.IsGrounded ? _crouchSpeed : _moveSpeed;
        
        _characterController.Move(moveVector * (speed * _deltaTime));
    }

    private void HorizontalAxisChanged(float value)
    {
        _horizontal = value;
    }
    
    private void VerticalAxisChanged(float value)
    {
        _vertical = value;
    }

    public void ChangeMatchState(MatchState matchState)
    {
        _matchState = matchState;
    }

    public void Cleanup()
    {
        _horizontalAxis.OnAxisChanged -= HorizontalAxisChanged;
        _verticalAxis.OnAxisChanged -= VerticalAxisChanged;
    }
}