using System;
using System.Collections;
using DG.Tweening;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


public sealed class Weapon : IWeapon
{
    private readonly WeaponView _weaponView;
    private readonly PlayerView _playerView;
    private readonly TracerFactory _tracerFactory;
    private readonly LayerMask _hitLayerMask;
    private readonly Transform _cameraTransform;
    private readonly HudView _hudView;
    private readonly Camera _camera;

    private Vector3 _newWeaponRotation;
    private Vector3 _newWeaponRotationVelocity;

    private IDisposable _reloadingRoutine;
    private Vector3 _targetWeaponRotation;
    private Vector3 _targetWeaponRotationVelocity;
    private float _deltaTime;
    private bool _isReloading;
    private bool _isReadyToShoot = true;
    private bool _isDead;
    private int _ammo;

    private readonly float _tracerFadeMultiplier;
    private readonly float _maxShotDistance;
    private readonly int _maxAmmo;

    private const float COLOR_FADE_MULTIPLIER = 15.0f;
    private const float WEAPON_SWAY_AMOUNT = 0.75f;
    private const float WEAPON_SWAY_SMOOTHING = 0.1f;
    private const float WEAPON_SWAY_RESET_SMOOTHING = 0.1f;
    private const float SWAY_CLAMP_X = 4.0f;
    private const float SWAY_CLAMP_Y = 4.0f;

    public GameObject Instance { get; }
    public Transform Barrel { get; private set; }
    public bool IsActive { get; private set; }
    public float Damage { get; }
    public float ShootCooldown { get; set; }
    public float ReloadTime { get; set; }
    public bool IsFullAuto { get; set; } = true;

    public Weapon(IWeaponFactory factory, IWeaponData data,
        CameraModel cameraModel, PlayerModel playerModel, HudView hudView)
    {
        _playerView = playerModel.PlayerView;
        playerModel.DeadChanged += OnPlayerDeadChanged;
        
        _hitLayerMask = data.HitLayerMask;
        Damage = data.Damage;
        ShootCooldown = data.ShootCooldown;
        ReloadTime = data.ReloadTime;
        _maxAmmo = data.MaxAmmo;
        _tracerFadeMultiplier = data.TracerFadeMultiplier;
        _maxShotDistance = data.MaxShotDistance;

        Instance = factory.Create(data);
        _weaponView = factory.WeaponView;
        Barrel = factory.Barrel.transform;

        _cameraTransform = cameraModel.CameraTransform;
        _camera = cameraModel.Camera;

        Instance.transform.parent = _cameraTransform;
        Instance.transform.localPosition = data.Position;

        _newWeaponRotation = Instance.transform.localRotation.eulerAngles;

        _tracerFactory = new TracerFactory(data);

        _hudView = hudView;

        Deactivate();

        _ammo = _maxAmmo;
        _hudView.SetAmmo(_ammo, _maxAmmo);
    }

    private void OnPlayerDeadChanged(bool isDead)
    {
        _isDead = isDead;

        if (_isDead)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
    }

    public void Fire()
    {
        if (!IsActive || !_isReadyToShoot || _isReloading || _isDead)
            return;

        if (_ammo <= 0)
        {
            _weaponView.PlayEmptyClickAudio();
            return;
        }


        var line = CreateTracer();

        _weaponView.PlayShotAudio();

        TweenLineWidth(line).ToObservable().Subscribe();
        StartShootCooldown().ToObservable().Subscribe();

        _ammo -= 1;
        _hudView.SetAmmo(_ammo, _maxAmmo);
    }

    private LineRenderer CreateTracer()
    {
        _tracerFactory.Create();
        var line = _tracerFactory.LineRenderer;

        line.SetPosition(0, Barrel.position);

        var ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out var hit, _maxShotDistance, _hitLayerMask))
        {
            line.SetPosition(1, hit.point);
            TryDamage(hit);
        }
        else
        {
            line.SetPosition(1, _cameraTransform.localPosition + _cameraTransform.forward * _maxShotDistance);
        }

        return line;
    }

    public void AutoFire()
    {
        if (_ammo <= 0)
            return;

        if (IsFullAuto)
        {
            Fire();
        }
    }

    public void Reload()
    {
        if (_isReloading || _isDead)
            return;

        _reloadingRoutine = StartReloading().ToObservable().Subscribe();
    }

    private IEnumerator StartReloading()
    {
        _isReloading = true;
        SpinWeapon();

        _weaponView.PlayReloadAudio();

        yield return new WaitForSeconds(ReloadTime);

        _isReloading = false;
        _ammo = _maxAmmo;
        _hudView.SetAmmo(_ammo, _maxAmmo);
    }

    private void SpinWeapon()
    {
        var localEulerAngles = Instance.transform.localEulerAngles;
        var startRotation = localEulerAngles;
        startRotation.x = -1.0f;
        localEulerAngles = startRotation;
        Instance.transform.localEulerAngles = localEulerAngles;
        Instance.transform.DOLocalRotate(new Vector3(-359, localEulerAngles.y), ReloadTime);
    }

    private void TryDamage(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out IDamageable enemy))
        {
            if (enemy.CheckIfMine())
            {
                enemy.Damage(Damage, _playerView);
            }
            else
            {
                _playerView.SendIdToDamage(enemy.ID, Damage);
            }
        }
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
            color.a -= _deltaTime * _tracerFadeMultiplier * COLOR_FADE_MULTIPLIER;
            line.material.color = color;

            yield return 0;
        }

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Destroy(line.gameObject);
        else
            Object.Destroy(line.gameObject);
    }

    private IEnumerator StartShootCooldown()
    {
        _isReadyToShoot = false;
        
        var timer = 0.0f;
        while (timer < ShootCooldown)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _isReadyToShoot = true;
    }

    public void Rotate(float mouseX, float mouseY, float deltaTime)
    {
        if (!IsActive || _isReloading || !Instance)
            return;

        _targetWeaponRotation.y += WEAPON_SWAY_AMOUNT * mouseX;
        _targetWeaponRotation.x += WEAPON_SWAY_AMOUNT * -mouseY;

        _targetWeaponRotation.x = Mathf.Clamp(_targetWeaponRotation.x, -SWAY_CLAMP_X, SWAY_CLAMP_X);
        _targetWeaponRotation.y = Mathf.Clamp(_targetWeaponRotation.y, -SWAY_CLAMP_Y, SWAY_CLAMP_Y);

        _targetWeaponRotation = Vector3.SmoothDamp(_targetWeaponRotation, Vector3.zero,
            ref _targetWeaponRotationVelocity, WEAPON_SWAY_RESET_SMOOTHING);
        _newWeaponRotation = Vector3.SmoothDamp(_newWeaponRotation, _targetWeaponRotation,
            ref _newWeaponRotationVelocity, WEAPON_SWAY_SMOOTHING);

        Instance.transform.localRotation = Quaternion.Euler(_newWeaponRotation);
    }

    public void Activate()
    {
        IsActive = true;

        _weaponView.EnableRenderers(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        _ammo = _maxAmmo;

        _weaponView.EnableRenderers(false);
    }

    public void Cleanup()
    {
        _reloadingRoutine?.Dispose();
    }
}