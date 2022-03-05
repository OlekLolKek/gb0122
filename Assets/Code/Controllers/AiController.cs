using System.Collections.Generic;
using UnityEngine;


public sealed class AiController : IInitialization, IExecutable, ICleanable
{
    private readonly List<BotController> _botControllers = new List<BotController>();
    private readonly BotSpawnPointView[] _spawnPositions;
    private readonly BotFactory _botFactory;

    public AiController(BotData botData, IWeaponData weaponData)
    {
        _spawnPositions = Object.FindObjectsOfType<BotSpawnPointView>();
        
        _botFactory = new BotFactory(botData, _spawnPositions);

        CreateBots(botData, weaponData);
    }

    public void Initialize()
    {
        foreach (var botController in _botControllers)
        {
            botController.Initialize();
        }
    }

    private void CreateBots(BotData botData, IWeaponData weaponData)
    {
        for (var i = 0; i < botData.BotAmount; ++i)
        {
            _botControllers.Add(new BotController(botData, _botFactory, int.MinValue + i,
                _spawnPositions, weaponData));
        }
    }

    public void Execute(float deltaTime)
    {
        foreach (var botController in _botControllers)
        {
            botController.Execute(deltaTime);
        }
    }

    public void Cleanup()
    {
        foreach (var botController in _botControllers)
        {
            botController.Cleanup();
        }
    }
}