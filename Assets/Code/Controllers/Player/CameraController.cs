using UnityEngine;


public sealed class CameraController : IExecutable
{
    private readonly Transform _cameraTransform;
    private readonly Transform _playerTransform;
    private readonly Transform _headTransform;
    
    private readonly float _sensitivity;
    
    private float _xRotation;
    private float _mouseX;
    private float _mouseY;

    private const float MIN_X_ROTATION = -90.0f;
    private const float MAX_X_ROTATION = 90.0f;

    public CameraController(CameraModel cameraModel, CameraData cameraData,
        PlayerModel playerModel, InputModel inputModel)
    {
        _cameraTransform = cameraModel.CameraTransform;
        _playerTransform = playerModel.Transform;
        _headTransform = playerModel.Head.transform;

        _sensitivity = cameraData.Sensitivity;

        var mouseX = inputModel.MouseX;
        var mouseY = inputModel.MouseY;
        mouseX.OnAxisChanged += ChangeMouseX;
        mouseY.OnAxisChanged += ChangeMouseY;
    }


    public void Execute(float deltaTime)
    {
        if (!_cameraTransform || !_headTransform )
        {
            return;
        }
        
        MoveCamera();
        RotateCamera();
    }

    private void MoveCamera()
    {
        _cameraTransform.position = _headTransform.position;
    }

    private void RotateCamera()
    {
        var mouseX = _mouseX * _sensitivity;
        var mouseY = _mouseY * _sensitivity;

        var rotation = _cameraTransform.localRotation.eulerAngles;
        var desiredX = rotation.y + mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, MIN_X_ROTATION, MAX_X_ROTATION);
            
        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, desiredX, 0.0f);
        _playerTransform.localRotation = Quaternion.Euler(0.0f, desiredX, 0.0f);
    }

    private void ChangeMouseX(float value)
    {
        _mouseX = value;
    }
        
    private void ChangeMouseY(float value)
    {
        _mouseY = value;
    }
}