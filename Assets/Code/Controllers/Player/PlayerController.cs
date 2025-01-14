﻿using Photon.Pun;


public sealed class PlayerController : IExecutable, IFixedExecutable, IMatchStateListener, ICleanable
{
    private readonly Controllers _controllers;

    public PlayerController(PlayerModel playerModel, InputModel inputModel,
        PlayerData playerData, HudView hudView, PlayerSpawnPointView[] spawnPoints)
    {
        _controllers = new Controllers();

        if (playerModel.PhotonView.IsMine || !PhotonNetwork.IsConnected)
        {
            var moveController = new MoveController(playerModel, playerData,
                inputModel);
            var jumpController = new JumpController(playerModel, playerData,
                inputModel);
            var crouchController = new CrouchController(inputModel, playerModel,
                playerData);
            var healthController = new HealthController(playerModel, playerData,
                hudView, spawnPoints, playerData.ScoreForDeath);
            var scoreController = new PlayerScoreController(playerModel.PlayerView,
                hudView);
        
            _controllers.Add(moveController);
            _controllers.Add(jumpController);
            _controllers.Add(crouchController);
            _controllers.Add(healthController);
            _controllers.Add(scoreController);
        }
    }
    
    public void Execute(float deltaTime)
    {
        _controllers.Execute(deltaTime);
    }

    public void FixedExecute()
    {
        _controllers.FixedExecute();
    }

    public void ChangeMatchState(MatchState matchState)
    {
        _controllers.ChangeMatchState(matchState);
    }

    public void Cleanup()
    {
        _controllers.Cleanup();
    }
}