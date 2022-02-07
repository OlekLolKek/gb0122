using Photon.Pun;
using UnityEngine;


public sealed class WeaponFactory : IWeaponFactory
{
    public Transform BarrelTransform { get; private set; }
    public Transform ScopeRailTransform { get; private set; }
    public AudioSource AudioSource { get; private set; }

    public GameObject Create(IWeaponData data)
    {
        var gun = PhotonNetwork.Instantiate(data.Prefab.name, Vector3.zero, Quaternion.identity);
        var view = gun.GetComponentInChildren<WeaponView>();
        BarrelTransform = view.Muzzle.transform;
        ScopeRailTransform = view.ScopeRail.transform;
        AudioSource = view.ShotAudioSource;

        return gun.gameObject;
    }
}