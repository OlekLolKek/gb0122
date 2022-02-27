using UnityEngine;


public interface IWeaponFactory
{
    public WeaponView WeaponView { get; }

    GameObject Create(IWeaponData data);
}