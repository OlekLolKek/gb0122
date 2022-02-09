using System;
using System.Collections.Generic;


public class WeaponInventory
{
    //TODO: replace 3 with a field
    private IWeapon[] _weapons = new IWeapon[3];
    private int _activeWeaponID;

    public IWeapon ActiveWeapon
    {
        get
        {
            return _weapons[_activeWeaponID];
        }
    }

    public void AddWeapon(IWeapon weapon)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i] == null)
            {
                _weapons[i] = weapon;
                return;
            }
        }

        _weapons[_activeWeaponID] = weapon;
    }

    public void SwitchWeapons(int id)
    {
        if (_weapons[id] == null) 
            return;
        if (id >= _weapons.Length)
            throw new ArgumentOutOfRangeException($"{nameof(id)} is out of inventory's range.");

        _weapons[_activeWeaponID].Deactivate();
        _activeWeaponID = id;
        _weapons[_activeWeaponID].Activate();
    }

        
    //TODO: replace with actual dropping instead of deleting
    public void DropWeapon(IWeapon weapon)
    {
        _weapons[_activeWeaponID] = null;
    }
}