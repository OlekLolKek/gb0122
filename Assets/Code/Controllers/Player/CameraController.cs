using DG.Tweening;
using UnityEngine;


public sealed class CameraController : IExecutable
{
    private readonly Transform _cameraTransform;
    private readonly Transform _playerTransform;
    private readonly Transform _headTransform;
    private readonly Transform _cameraParent;

    private readonly float _sensitivity;

    private Tweener _shakeTweener;

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
        _cameraParent = cameraModel.CameraParent;

        _sensitivity = cameraData.Sensitivity;

        playerModel.OnHealthChanged += PlayerHealthChanged;

        var mouseX = inputModel.MouseX;
        var mouseY = inputModel.MouseY;
        mouseX.OnAxisChanged += ChangeMouseX;
        mouseY.OnAxisChanged += ChangeMouseY;
    }

    public void Execute(float deltaTime)
    {
        if (!_cameraTransform || !_headTransform || !_cameraParent)
        {
            return;
        }

        MoveCamera();
        RotateCamera();
    }

    private void PlayerHealthChanged(float _)
    {
        _cameraTransform.DOShakePosition(0.125f, 0.175f, 35).OnComplete(ResetCameraPosition);
    }

    private void ResetCameraPosition()
    {
        _cameraTransform.localPosition = Vector3.zero;
    }

    private void MoveCamera()
    {
        _cameraParent.position = _headTransform.position;
    }

    private void RotateCamera()
    {
        var mouseX = _mouseX * _sensitivity;
        var mouseY = _mouseY * _sensitivity;

        var rotation = _cameraParent.localRotation.eulerAngles;
        var desiredX = rotation.y + mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, MIN_X_ROTATION, MAX_X_ROTATION);

        _cameraParent.localRotation = Quaternion.Euler(_xRotation, desiredX, 0.0f);
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