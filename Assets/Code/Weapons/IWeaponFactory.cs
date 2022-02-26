using UnityEngine;


public interface IWeaponFactory
{
    Transform BarrelTransform { get; }
    Transform ScopeRailTransform { get; }
    AudioSource ShotAudioSource { get; }
    AudioSource EmptyClickAudioSource { get; }
    AudioSource ReloadAudioSource { get; }

    GameObject Create(IWeaponData data);
}