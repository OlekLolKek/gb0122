public sealed class PlayerController : IExecutable, ICleanable
{
    private readonly Controllers _controllers;

    public PlayerController(PlayerModel playerModel, InputModel inputModel,
        PlayerData playerData)
    {
        _controllers = new Controllers();
            
        var collisionModel = new PlayerJumpModel();
        var crouchModel = new PlayerCrouchModel();
            
        var playerCollisionController = new PlayerCollisionController(playerModel, collisionModel,
            playerData);
        var moveController = new MoveController(playerModel, playerData, 
            inputModel, collisionModel, 
            crouchModel);
        var jumpController = new JumpController(playerModel, playerData, 
            inputModel, collisionModel);
        var crouchController = new CrouchController(inputModel, crouchModel,
            collisionModel, playerModel,
            playerData);

        _controllers.Add(playerCollisionController);
        _controllers.Add(moveController);
        _controllers.Add(jumpController);
        _controllers.Add(crouchController);
    }
        
    public void Execute(float deltaTime)
    {
        _controllers.Execute(deltaTime);
    }

    public void Cleanup()
    {
        _controllers.Cleanup();
    }
}