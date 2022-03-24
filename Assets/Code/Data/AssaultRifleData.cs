using UnityEngine;


[CreateAssetMenu(fileName = "AssaultRifleData", menuName = "Data/Weapon/AssaultRifleData")]
public sealed class AssaultRifleData : ScriptableObject, IWeaponData
{
    [SerializeField] private WeaponView _prefab;
    [SerializeField] private TracerView _tracerPrefab;
    [SerializeField] private Material _tracerMaterial;
    [SerializeField] private LayerMask _hitLayerMask;
    [SerializeField] private Vector3 _position;
    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _emptyAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;

    [SerializeField] private string _tracerName;
    [SerializeField] private float _tracerFadeMultiplier;
    [SerializeField] private float _maxShotDistance;
    [SerializeField] private float _tracerWidth;
    [SerializeField] private float _shootCooldown;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _damage;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _otherWeaponsLayer;


    public WeaponView Prefab => _prefab;
    public TracerView TracerPrefab => _tracerPrefab;
    public Material TracerMaterial => _tracerMaterial;
    public LayerMask HitLayerMask => _hitLayerMask;
    public Vector3 Position => _position;
    public AudioClip ShotAudioClip => _shotAudioClip;
    public AudioClip EmptyAudioClip => _emptyAudioClip;
    public AudioClip ReloadAudioClip => _reloadAudioClip;
    public string TracerName => _tracerName;
    public float ShootCooldown => _shootCooldown;
    public float ReloadTime => _reloadTime;
    public float Damage => _damage;
    public float TracerWidth => _tracerWidth;
    public float TracerFadeMultiplier => _tracerFadeMultiplier;
    public float MaxShotDistance => _maxShotDistance;
    public int MaxAmmo => _maxAmmo;
    public int OtherWeaponsLayer => _otherWeaponsLayer;
}