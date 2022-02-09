using UnityEngine;


public sealed class CameraModel
{
    public Transform CameraTransform { get; }
    public Camera Camera { get; }

    public CameraModel(CameraFactory factory)
    {
        factory.Create();
        CameraTransform = factory.CameraTransform;
        Camera = factory.Camera;
    }
}