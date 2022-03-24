using Photon.Pun;
using UnityEngine;


public sealed class WeaponFactory : IWeaponFactory
{
    public WeaponView WeaponView { get; private set; }
    public GameObject Barrel { get; private set; }

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
        Barrel = WeaponView.Muzzle;
        WeaponView.SetLayer(data.OtherWeaponsLayer);

        return gun.gameObject;
    }
}