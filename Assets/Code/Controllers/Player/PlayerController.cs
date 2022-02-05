public sealed class PlayerController : IExecutable, IFixedExecutable, ICleanable
{
    private readonly Controllers _controllers;

    public PlayerController(PlayerModel playerModel, InputModel inputModel,
        PlayerData playerData)
    {
        _controllers = new Controllers();
        
        var moveController = new MoveController(playerModel, playerData, 
            inputModel);
        var jumpController = new JumpController(playerModel, playerData, 
            inputModel);
        var crouchController = new CrouchController(inputModel, playerModel,
            playerData);
        
        _controllers.Add(moveController);
        _controllers.Add(jumpController);
        _controllers.Add(crouchController);
    }
        
    public void Execute(float deltaTime)
    {
        _controllers.Execute(deltaTime);
    }

    public void FixedExecute()
    {
        _controllers.FixedExecute();
    }

    public void Cleanup()
    {
        _controllers.Cleanup();
    }
}