using Photon.Pun;
using UnityEngine;


public sealed class WeaponFactory : IWeaponFactory
{
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
        
        var view = gun.GetComponentInChildren<WeaponView>();
        BarrelTransform = view.Muzzle.transform;
        ScopeRailTransform = view.ScopeRail.transform;
        ShotAudioSource = view.ShotAudioSource;
        EmptyClickAudioSource = view.EmptyClickAudioSource;
        ReloadAudioSource = view.ReloadAudioSource;

        return gun.gameObject;
    }
}