using UnityEngine;


public abstract class WeaponModification : IWeapon
{
    private IWeapon _weapon;
    private bool _isApplied;

    public GameObject Instance => _weapon.Instance;


    protected abstract IWeapon AddModification(IWeapon weapon);
    protected abstract IWeapon RemoveModification(IWeapon weapon);

    public void SwitchModifications(IWeapon weapon)
    {
        if (_isApplied)
        {
            _weapon = RemoveModification(weapon);
            _isApplied = false;
        }
        else
        {
            _weapon = AddModification(weapon);
            _isApplied = true;
        }
    }

    public void Fire()
    {
        _weapon.Fire();
    }

    public void Activate()
    {
        _weapon.Activate();
    }

    public void Deactivate()
    {
        _weapon.Deactivate();
    }

    public void Rotate(float mouseX, float mouseY, float deltaTime)
    {
        _weapon.Rotate(mouseX, mouseY, deltaTime);
    }

    public void SetModdedValues(Transform barrel, AudioSource audioSource)
    {
        _weapon.SetModdedValues(barrel, audioSource);
    }

    public void SetDefaultValues()
    {
        _weapon.SetDefaultValues();
    }

    public void Execute(float deltaTime)
    {
        _weapon.Execute(deltaTime);
    }
}