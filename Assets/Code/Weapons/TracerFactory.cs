using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;


public sealed class TracerFactory : IFactory
{
    public GameObject GameObject { get; private set; }
    public LineRenderer LineRenderer { get; private set; }

    private IWeaponData _data;
        
    public TracerFactory(IWeaponData data)
    {
        _data = data;
    }
        
    public GameObject Create()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameObject = PhotonNetwork.Instantiate(_data.TracerPrefab.name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            GameObject = Object.Instantiate(_data.TracerPrefab, Vector3.zero, Quaternion.identity).gameObject;
        }
        
        
        var view = GameObject.GetComponentInChildren<TracerView>();
        LineRenderer = view.LineRenderer;
            
        LineRenderer.positionCount = 2;
        LineRenderer.startWidth = _data.TracerWidth;
        LineRenderer.endWidth = _data.TracerWidth;
        LineRenderer.generateLightingData = true;
        LineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        LineRenderer.receiveShadows = false;
        LineRenderer.material = _data.TracerMaterial;

        return GameObject;
    }
}