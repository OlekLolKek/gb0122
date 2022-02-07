using System.IO;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon/WeaponData")]
public sealed class WeaponData : ScriptableObject
{
    [SerializeField] private string _weaponDataRootPath;
        
    [SerializeField] private string _assaultRifleSilencerDataPath;
    [SerializeField] private string _assaultRifleScopeDataPath;
    [SerializeField] private string _safetyDataPath;
    [SerializeField] private string _assaultRifleDataPath;

    private BarrelAttachmentData _assaultRifleSilencerData;
    private ScopeData _assaultRifleScopeData;
    private SafetyData _safetyData;
    private AssaultRifleData _assaultRifleData;

    public BarrelAttachmentData AssaultRifleSilencerData
    {
        get
        {
            if (_assaultRifleSilencerData == null)
            {
                _assaultRifleSilencerData =
                    Load<BarrelAttachmentData>(_weaponDataRootPath + _assaultRifleSilencerDataPath);
            }

            return _assaultRifleSilencerData;
        }
    }
        
    public ScopeData AssaultRifleScopeData
    {
        get
        {
            if (_assaultRifleScopeData == null)
            {
                _assaultRifleScopeData = Load<ScopeData>(_weaponDataRootPath + _assaultRifleScopeDataPath);
            }

            return _assaultRifleScopeData;
        }
    }

    public SafetyData SafetyData
    {
        get
        {
            if (_safetyData == null)
            {
                _safetyData = Load<SafetyData>(_weaponDataRootPath + _safetyDataPath);
            }

            return _safetyData;
        }
    }
        
    public AssaultRifleData AssaultRifleData
    {
        get
        {
            if (_assaultRifleData == null)
            {
                _assaultRifleData = Load<AssaultRifleData>(_weaponDataRootPath + _assaultRifleDataPath);
            }

            return _assaultRifleData;
        }
    }


    private T Load<T>(string resourcesPath) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
    }
}