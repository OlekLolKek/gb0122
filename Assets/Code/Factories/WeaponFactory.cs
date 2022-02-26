using Photon.Pun;
using UnityEngine;


public sealed class WeaponFactory : IWeaponFactory
{
    public WeaponView WeaponView { get; private set; }
    public Transform BarrelTransform { get; private set; }
    public Transform ScopeRailTransform { get; private set; }
    public AudioSource ShotAudioSource { get; private set; }
    public AudioSource EmptyClickAudioSource { get; private set; }
    public AudioSource ReloadAudioSource { get; private set; }

    public GameObject Create(IWeaponData data)
    {
        GameObject gun;
        
        if (PhotonNetwork.IsConnected)
        {
            gun = PhotonNetwork.Instantiate(data.Prefab.name, Vector3.zero, Quaternion.identity);
        }
        else
        { 
            gun = Object.Instantiate(data.Prefab, Vector3.zero, Quaternion.identity).gameObject;
        }
        
        WeaponView = gun.GetComponentInChildren<WeaponView>();
        BarrelTransform = WeaponView.Muzzle.transform;
        ScopeRailTransform = WeaponView.ScopeRail.transform;
        ShotAudioSource = WeaponView.ShotAudioSource;
        EmptyClickAudioSource = WeaponView.EmptyClickAudioSource;
        ReloadAudioSource = WeaponView.ReloadAudioSource;

        return gun.gameObject;
    }
}