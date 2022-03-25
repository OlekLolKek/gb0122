using UnityEngine;


public sealed class CameraModel
{
    public Transform CameraTransform { get; }
    public Transform CameraParent { get; }
    public Camera Camera { get; }

    public CameraModel(CameraFactory factory)
    {
        factory.Create();
        CameraTransform = factory.CameraTransform;
        CameraParent = factory.CameraParent;
        Camera = factory.Camera;
    }
}