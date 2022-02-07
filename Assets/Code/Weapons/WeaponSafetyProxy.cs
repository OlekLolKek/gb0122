using UnityEngine;


public class WeaponSafetyProxy : IWeapon
{
    private readonly IWeapon _weapon;
    private readonly AudioSource _audioSource;
    private bool _isSafetyOn;

    public GameObject Instance => _weapon.Instance;

    public WeaponSafetyProxy(IWeapon weapon, SafetyFactory factory)
    {
        factory.Create();
            
        _weapon = weapon;
        _audioSource = factory.AudioSource;
        _audioSource.transform.parent = Instance.transform;
    }

    public void Fire()
    {
        if (_isSafetyOn)
        {
            _audioSource.Play();
        }
        else
        {
            _weapon.Fire();
        }
    }

    public void SwitchSafety()
    {
        _isSafetyOn = !_isSafetyOn;
        _audioSource.Play();
    }

    public void Activate()
    {
        _weapon.Activate();
    }

    public void Deactivate()
    {
        _weapon.Deactivate();
    }

    public void Rotate(float mouseX, float mouseY)
    {
        _weapon.Rotate(mouseX, mouseY);
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