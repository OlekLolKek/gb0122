using System.Collections;
using Photon.Pun;
using UniRx;
using UnityEngine;


public sealed class Weapon : IWeapon
{
    private readonly AudioSource _baseAudioSource;
    private readonly TracerFactory _tracerFactory;
    private readonly LayerMask _hitLayerMask;
    private readonly Transform _cameraTransform;
    private readonly Transform _baseBarrel;
    private readonly Camera _camera;
    private readonly Vector3 _position;
    
    private float _deltaTime;

    private readonly float _tracerFadeMultiplier;
    private readonly float _colorFadeMultiplier = 15;
    private readonly float _maxShotDistance;

    public GameObject Instance { get; }
    public Transform ScopeRail { get; private set; }
    public Transform Barrel { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public bool IsActive { get; private set; }
    public float Damage { get; set; }
    public float ShootCooldown { get; set; }
    public float ReloadTime { get; set; }
    public bool IsFullAuto { get; set; }


    public Weapon(IWeaponFactory factory, IWeaponData data,
        CameraModel cameraModel)
    {
        _position = data.Position;
        _hitLayerMask = data.HitLayerMask;
        Damage = data.Damage;
        ShootCooldown = data.ShootCooldown;
        _tracerFadeMultiplier = data.TracerFadeMultiplier;
        _maxShotDistance = data.MaxShotDistance;

        Instance = factory.Create(data);
        Barrel = factory.BarrelTransform;
        _baseBarrel = Barrel;
        ScopeRail = factory.ScopeRailTransform;
        AudioSource = factory.AudioSource;
        _baseAudioSource = AudioSource;

        _cameraTransform = cameraModel.CameraTransform;
        _camera = cameraModel.Camera;

        Instance.transform.parent = _cameraTransform;
        Instance.transform.localPosition = _position;

        _tracerFactory = new TracerFactory(data);
            
        Deactivate();
    }

    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
    }

    public void Fire()
    {
        if (!IsActive) return;

        _tracerFactory.Create();
        var line = _tracerFactory.LineRenderer;

        line.SetPosition(0, Barrel.position);

        var ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out var hit, _maxShotDistance, _hitLayerMask))
        {
            line.SetPosition(1, hit.point);
        }
        else
        {
            line.SetPosition(1, _cameraTransform.localPosition + _cameraTransform.forward * _maxShotDistance);
        }

        AudioSource.Play();

        TweenLineWidth(line).ToObservable().Subscribe();
    }

    private IEnumerator TweenLineWidth(LineRenderer line)
    {
        while (line.endWidth > 0)
        {
            var endWidth = line.endWidth;
            endWidth -= _deltaTime * _tracerFadeMultiplier;
            line.endWidth = endWidth;
            line.startWidth = endWidth;

            var color = line.material.color;
            color.a -= _deltaTime * _tracerFadeMultiplier * _colorFadeMultiplier;
            line.material.color = color;

            yield return 0;
        }

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Destroy(line.gameObject);
        else
            Object.Destroy(line.gameObject);
    }

    public void Rotate(float mouseX, float mouseY)
    {
        if (!IsActive) return;
        var rotation = Vector3.zero;
        rotation.y += mouseX;
        rotation.x -= mouseY;
        Instance.transform.localEulerAngles = rotation;
    }

    public void SetModdedValues(Transform barrel, AudioSource audioSource)
    {
        Barrel = barrel;
        AudioSource = audioSource;
    }

    public void SetDefaultValues()
    {
        Barrel = _baseBarrel;
        AudioSource = _baseAudioSource;
    }
        
    public void Activate()
    {
        IsActive = true;
        Instance.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        Instance.SetActive(false);
    }
}