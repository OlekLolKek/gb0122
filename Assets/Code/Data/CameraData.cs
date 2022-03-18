using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[CreateAssetMenu(fileName = "CameraData", menuName = "Data/CameraData")]
public sealed class CameraData : ScriptableObject, IData
{
    [SerializeField] private PostProcessResources _postProcessResources;
    [SerializeField] private LayerMask _postProcessingLayer;
    [SerializeField] private LayerMask _cullingLayerMask;
    [SerializeField] private LayerMask _weaponCameraCullingLayerMask;
    [SerializeField] private string _cameraName;
    [SerializeField] private string _weaponCameraName;
    [SerializeField, Range(1, 179)] private float _fov;
    [SerializeField, Range(0.01f, 5000)] private float _clippingPlaneFar;
    [SerializeField, Range(0.01f, 5000)] private float _clippingPlaneNear;
    [SerializeField] private float _sensitivity;


    public PostProcessResources PostProcessResources => _postProcessResources;
    public LayerMask PostProcessingLayer => _postProcessingLayer;
    public LayerMask CullingLayerMask => _cullingLayerMask;
    public LayerMask WeaponCameraCullingLayerMask => _weaponCameraCullingLayerMask;
    public string CameraName => _cameraName;
    public string WeaponCameraName => _weaponCameraName;
    public float FOV => _fov;
    public float ClippingPlaneFar => _clippingPlaneFar;
    public float ClippingPlaneNear => _clippingPlaneNear;
    public float Sensitivity => _sensitivity;
}