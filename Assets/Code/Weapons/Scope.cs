using UniRx.Triggers;
using UnityEngine;


public sealed class Scope : IScope
{
    private readonly Transform _weaponRail;
        
    public GameObject Instance { get; }
    public bool IsActive { get; set; }


    public Scope(ScopeFactory factory, Weapon weapon)
    {
        Instance = factory.Create();
        _weaponRail = weapon.ScopeRail;
        Instance.transform.parent = _weaponRail;
        Instance.transform.localPosition = Vector3.zero;
        Instance.transform.localRotation = _weaponRail.localRotation;
        Deactivate();
    }
        
    public void Activate()
    {
        IsActive = true;
        Instance.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        Instance.SetActive(false);
    }
}