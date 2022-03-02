using Photon.Pun;
using UnityEngine;


public sealed class BotFactory : IFactory
{
    public BotView BotView { get; private set; }
    
    private readonly BotData _botData;

    private readonly BotSpawnPointView[] _spawnPositions;

    public BotFactory(BotData botData, BotSpawnPointView[] spawnPositions)
    {
        _botData = botData;

        _spawnPositions = spawnPositions;
    }
    
    public GameObject Create()
    {
        GameObject bot;
        var spawnPoint = _spawnPositions[Random.Range(0, _spawnPositions.Length)].transform;

        var xOffset = Random.Range(-2, 2);
        var zOffset = Random.Range(-2, 2);

        var position = spawnPoint.position;
        position.x += xOffset;
        position.z += zOffset;

        if (PhotonNetwork.IsConnected)
        {
            bot = PhotonNetwork.Instantiate(_botData.BotPrefab.name,
                position, spawnPoint.rotation);
        }
        else
        {
            bot = Object.Instantiate(_botData.BotPrefab,
                position, spawnPoint.rotation).gameObject;
        }

        BotView = bot.GetComponentInChildren<BotView>();

        return bot;
    }
}