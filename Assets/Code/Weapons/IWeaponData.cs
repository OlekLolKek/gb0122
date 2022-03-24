using UnityEngine;


public interface IWeaponData : IData
{
    WeaponView Prefab { get; }
    TracerView TracerPrefab { get; }
    Material TracerMaterial { get; }
    LayerMask HitLayerMask { get; }
    Vector3 Position { get; }
    float ShootCooldown { get; }
    float ReloadTime { get; }
    float Damage { get; }
    float TracerWidth { get; }
    float TracerFadeMultiplier { get; }
    float MaxShotDistance { get; }
    int MaxAmmo { get; }
    int OtherWeaponsLayer { get; }
}