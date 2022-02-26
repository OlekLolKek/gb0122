public sealed class WeaponController : IExecutable, ICleanable
{
    private readonly IInputKeyPress _primary;
    private readonly IInputKeyPress _secondary;
    private readonly IInputKeyPress _melee;
    private readonly IInputKeyPress _singleFire;
    private readonly IInputKeyHold _autoFire;
    private readonly IInputKeyPress _reload;
    private readonly IInputAxisChange _mouseXInput;
    private readonly IInputAxisChange _mouseYInput;
    private readonly WeaponInventory _inventory;

    private float _mouseX;
    private float _mouseY;
        
    public WeaponController(InputModel inputModel, WeaponData data,
        CameraModel cameraModel, PlayerModel playerModel, HudView hudView)
    {
        _inventory = new WeaponInventory();
        var weaponFactory = new WeaponFactory();

        _primary = inputModel.Weapon1;
        _secondary = inputModel.Weapon2;
        _melee = inputModel.Weapon3;
        _mouseXInput = inputModel.MouseX;
        _mouseYInput = inputModel.MouseY;
        _singleFire = inputModel.SingleFire;
        _autoFire = inputModel.AutoFire;
        _reload = inputModel.Reload;

        _primary.OnKeyPressed += SelectPrimaryWeapon;
        _secondary.OnKeyPressed += SelectSecondaryWeapon;
        _melee.OnKeyPressed += SelectMeleeWeapon;
        _singleFire.OnKeyPressed += Shoot;
        _autoFire.OnKeyHeld += AutoFire;
        _mouseXInput.OnAxisChanged += MouseXChange;
        _mouseYInput.OnAxisChanged += MouseYChange;
        _reload.OnKeyPressed += Reload;

        var weapon = new Weapon(weaponFactory, data.AssaultRifleData, cameraModel, playerModel, hudView);
        _inventory.AddWeapon(weapon);
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

    private void AutoFire(bool isKeyHeld)
    {
        if (isKeyHeld)
        {
            _inventory.ActiveWeapon.AutoFire();
        }
    }

    private void Reload()
    {
        _inventory.ActiveWeapon.Reload();
    }

    public void Cleanup()
    {
        _primary.OnKeyPressed -= SelectPrimaryWeapon;
        _secondary.OnKeyPressed -= SelectSecondaryWeapon;
        _melee.OnKeyPressed -= SelectMeleeWeapon;
        _singleFire.OnKeyPressed -= Shoot;
        _autoFire.OnKeyHeld -= AutoFire;
        _mouseXInput.OnAxisChanged -= MouseXChange;
        _mouseYInput.OnAxisChanged -= MouseYChange;
        _reload.OnKeyPressed -= Reload;
    }
}