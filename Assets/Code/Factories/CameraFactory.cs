using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public sealed class CameraFactory : IFactory
{
    private readonly CameraData _cameraData;
    public Camera Camera { get; set; }
    public Camera WeaponCamera { get; set; }
    public Transform CameraTransform { get; set; }

    public CameraFactory(CameraData cameraData)
    {
        _cameraData = cameraData;
    }
        
    public GameObject Create()
    {
        var camera = new GameObject(_cameraData.CameraName);
            
        Camera = camera.AddComponent<Camera>();
        Camera.farClipPlane = _cameraData.ClippingPlaneFar;
        Camera.nearClipPlane = _cameraData.ClippingPlaneNear;
        Camera.fieldOfView = _cameraData.FOV;

        CameraTransform = Camera.transform;
            
        camera.AddComponent<AudioListener>();

        var postProcessing = camera.AddComponent<PostProcessLayer>();
        postProcessing.Init(_cameraData.PostProcessResources);
        postProcessing.volumeTrigger = camera.transform;
        postProcessing.volumeLayer = _cameraData.PostProcessingLayer;
        postProcessing.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;

        Camera.cullingMask = _cameraData.CullingLayerMask.value;

        var weaponCamera = new GameObject(_cameraData.WeaponCameraName);
        weaponCamera.transform.SetParent(CameraTransform);
            
        WeaponCamera = weaponCamera.AddComponent<Camera>();
        WeaponCamera.depth = 1;
        WeaponCamera.clearFlags = CameraClearFlags.Depth;
        WeaponCamera.cullingMask = _cameraData.WeaponCameraCullingLayerMask.value;
        WeaponCamera.fieldOfView = _cameraData.FOV;
        WeaponCamera.useOcclusionCulling = false;

        return Camera.gameObject;
    }
}