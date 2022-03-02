using UnityEngine;


public interface IWeaponFactory
{
    public WeaponView WeaponView { get; }
    public GameObject Barrel { get; }

    GameObject Create(IWeaponData data);
}