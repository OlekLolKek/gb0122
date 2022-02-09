public sealed class InputModel
{
    public IInputAxisChange Horizontal { get; }
    public IInputAxisChange Vertical { get; }
    public IInputAxisChange MouseX { get; }
    public IInputAxisChange MouseY { get; }
    public IInputKeyPress StartCrouch { get; }
    public IInputKeyRelease StopCrouch { get; }
    public IInputKeyHold Jump { get; }
    public IInputKeyPress Weapon1 { get; }
    public IInputKeyPress Weapon2 { get; }
    public IInputKeyPress Weapon3 { get; }
    public IInputKeyPress ChangeMod { get; }
    public IInputKeyPress Fire { get; }
    public IInputKeyPress Safety { get; }


    public InputModel(InputData inputData)
    {
        Horizontal = new PCInputAxis(AxisNameStorage.HORIZONTAL);
        Vertical = new PCInputAxis(AxisNameStorage.VERTICAL);
        MouseX = new PCInputAxis(AxisNameStorage.MOUSE_X);
        MouseY = new PCInputAxis(AxisNameStorage.MOUSE_Y);
        StartCrouch = new PCInputKeyDown(inputData.Crouch);
        StopCrouch = new PCInputKeyUp(inputData.Crouch);
        Jump = new PCInputKeyHold(inputData.Jump);
        Weapon1 = new PCInputKeyDown(inputData.Weapon1);
        Weapon2 = new PCInputKeyDown(inputData.Weapon2);
        Weapon3 = new PCInputKeyDown(inputData.Weapon3);
        ChangeMod = new PCInputKeyDown(inputData.ChangeMod);
        Fire = new PCInputKeyDown(inputData.Fire);
        Safety = new PCInputKeyDown(inputData.Safety);
    }
}