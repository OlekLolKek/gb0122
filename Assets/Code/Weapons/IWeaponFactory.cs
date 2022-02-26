using UnityEngine;


public interface IWeaponFactory
{
    Transform BarrelTransform { get; }
    Transform ScopeRailTransform { get; }
    AudioSource ShotAudioSource { get; }

    GameObject Create(IWeaponData data);
}