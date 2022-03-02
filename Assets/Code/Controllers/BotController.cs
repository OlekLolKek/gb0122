using System;
using System.Collections;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


public sealed class BotController : IExecutable, ICleanable
{
    private readonly BotSpawnPointView[] _spawnPoints;
    private readonly GameObject _instance;
    private readonly BotView _botView;

    private readonly float _respawnTime;
    private readonly float _maxHealth;
    private readonly float _damage;
    private float _health;

    private IDisposable _respawnCoroutine;

    private float _deltaTime;
    
    public BotController(BotData botData, BotFactory botFactory, int id,
        BotSpawnPointView[] spawnPoints)
    {
        _respawnTime = botData.BotRespawnTime;
        _maxHealth = botData.BotHealth;
        _health = botData.BotHealth;
        _damage = botData.BotDamage;

        _instance = botFactory.Create();
        _botView = botFactory.BotView;

        _botView.SetId(id);
        _botView.SetHealth(_health);
        _botView.OnReceivedDamage += OnBotDamaged;

        _spawnPoints = spawnPoints;
    }

    private (Vector3, Quaternion) GetRandomPosition()
    {
        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

        var xOffset = Random.Range(-2, 2);
        var zOffset = Random.Range(-2, 2);

        var position = spawnPoint.position;
        position.x += xOffset;
        position.z += zOffset;

        return (position, spawnPoint.rotation);
    }

    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
    }

    private void OnBotDamaged(float damage)
    {
        _health -= damage;
        _botView.SetHealth(_health);
        
        if (_health <= 0.0f)
        {
            _respawnCoroutine = Respawn().ToObservable().Subscribe();
        }
    }
    
    private IEnumerator Respawn()
    {
        _health = _maxHealth;
        _botView.SetHealth(_health);
        _botView.SetDead(true);
        _botView.NavMeshAgent.enabled = false;

        var (position, rotation) = GetRandomPosition();

        var transform = _botView.transform;
        transform.position = position;
        transform.rotation = rotation;

        yield return new WaitForSeconds(_respawnTime);
        
        _botView.SetDead(false);
        _botView.NavMeshAgent.enabled = true;
    }

    public void Cleanup()
    {
        _botView.OnReceivedDamage -= OnBotDamaged;
        
        _respawnCoroutine?.Dispose();
    }
}