using UnityEngine;


public interface IWeaponFactory
{
    public WeaponView WeaponView { get; }
    Transform BarrelTransform { get; }
    Transform ScopeRailTransform { get; }
    AudioSource ShotAudioSource { get; }
    AudioSource EmptyClickAudioSource { get; }
    AudioSource ReloadAudioSource { get; }

    GameObject Create(IWeaponData data);
}