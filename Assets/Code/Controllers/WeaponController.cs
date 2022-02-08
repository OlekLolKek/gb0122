public sealed class WeaponController : IExecutable, ICleanable
{
    private readonly IInputKeyPress _primary;
    private readonly IInputKeyPress _secondary;
    private readonly IInputKeyPress _melee;
    private readonly IInputKeyPress _fire;
    private readonly IInputKeyPress _changeMod;
    private readonly IInputKeyPress _switchSafety;
    private readonly IInputAxisChange _mouseXInput;
    private readonly IInputAxisChange _mouseYInput;
    private readonly WeaponInventory _inventory;
    private readonly WeaponModification _modification;
    private readonly WeaponSafetyProxy _arSafetyProxy;

    private float _mouseX;
    private float _mouseY;
        
    public WeaponController(InputModel inputModel, WeaponData data,
        CameraModel cameraModel, PlayerModel playerModel)
    {
        _inventory = new WeaponInventory();
        var weaponFactory = new WeaponFactory();
        var silencerFactory = new BarrelAttachmentFactory(data.AssaultRifleSilencerData);
        var scopeFactory = new ScopeFactory(data.AssaultRifleScopeData);

        _primary = inputModel.Weapon1;
        _secondary = inputModel.Weapon2;
        _melee = inputModel.Weapon3;
        _mouseXInput = inputModel.MouseX;
        _mouseYInput = inputModel.MouseY;
        _changeMod = inputModel.ChangeMod;
        _fire = inputModel.Fire;
        _switchSafety = inputModel.Safety;

        _primary.OnKeyPressed += SelectPrimaryWeapon;
        _secondary.OnKeyPressed += SelectSecondaryWeapon;
        _melee.OnKeyPressed += SelectMeleeWeapon;
        _fire.OnKeyPressed += Shoot;
        _changeMod.OnKeyPressed += ChangeModification;
        _mouseXInput.OnAxisChanged += MouseXChange;
        _mouseYInput.OnAxisChanged += MouseYChange;
        _switchSafety.OnKeyPressed += SwitchSafety;
        
        var weapon = new Weapon(weaponFactory, data.AssaultRifleData, cameraModel, playerModel);
        var safetyFactory = new SafetyFactory(data.SafetyData);
        _arSafetyProxy = new WeaponSafetyProxy(weapon, safetyFactory);
        _inventory.AddWeapon(_arSafetyProxy);
            
        var silencer = new BarrelAttachment(silencerFactory, weapon);
        var scope = new Scope(scopeFactory, weapon);
        _modification = new ModificationsController(silencer, scope);
    }

    public void Execute(float deltaTime)
    {
        var weapon = _inventory.ActiveWeapon;
        weapon.Rotate(_mouseX, _mouseY, deltaTime);
        weapon.Execute(deltaTime);
    }

    private void SelectPrimaryWeapon()
    {
        _inventory.SwitchWeapons(0);
    }
        
    private void SelectSecondaryWeapon()
    {
        _inventory.SwitchWeapons(1);
    }
        
    private void SelectMeleeWeapon()
    {
        _inventory.SwitchWeapons(2);
    }

    private void MouseXChange(float value)
    {
        _mouseX = value;
    }

    private void MouseYChange(float value)
    {
        _mouseY = value;
    }

    private void Shoot()
    {
        _inventory.ActiveWeapon.Fire();
    }

    private void ChangeModification()
    {
        _modification.SwitchModifications(_inventory.ActiveWeapon);
    }

    private void SwitchSafety()
    {
        _arSafetyProxy.SwitchSafety();
    }

    public void Cleanup()
    {
        _primary.OnKeyPressed -= SelectPrimaryWeapon;
        _secondary.OnKeyPressed -= SelectSecondaryWeapon;
        _melee.OnKeyPressed -= SelectMeleeWeapon;
        _fire.OnKeyPressed -= Shoot;
        _changeMod.OnKeyPressed -= ChangeModification;
        _mouseXInput.OnAxisChanged -= MouseXChange;
        _mouseYInput.OnAxisChanged -= MouseYChange;
        _switchSafety.OnKeyPressed -= SwitchSafety;
    }
}