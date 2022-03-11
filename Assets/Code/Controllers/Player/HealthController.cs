using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public sealed class HealthController : IExecutable, ICleanable
{
    private readonly PlayerSpawnPointView[] _spawnPoints;
    private readonly PlayerModel _playerModel;
    private readonly PlayerView _playerView;
    private readonly Renderer[] _renderers;
    private readonly HudView _hudView;
    private readonly float _respawnTime;
    private readonly string[] _deathMessages;


    private IDisposable _respawnCoroutine;
    private float _deltaTime;

    public HealthController(PlayerModel playerModel, PlayerData playerData,
        HudView hudView, PlayerSpawnPointView[] spawnPoints)
    {
        _spawnPoints = spawnPoints;
        
        _playerModel = playerModel;
        _playerView = playerModel.PlayerView;
        _respawnTime = playerData.RespawnTime;
        _deathMessages = playerData.DeathMessages;
        _hudView = hudView;

        _playerModel.MaxHealth = playerData.MaxHealth;
        _playerModel.Health = playerData.MaxHealth;

        _playerView.OnReceivedDamage += ReceivedDamage;
        _hudView.SetHealth(_playerModel.Health);
        _playerView.SetHealth(_playerModel.Health);

        _renderers = _playerModel.Transform.GetComponentsInChildren<Renderer>();
        
        var (position, rotation) = GetRandomPosition();

        Debug.Log($"Setting {_playerModel.Transform.name} position to {position}, rotation to {rotation}");
        _playerModel.Transform.position = position;
        _playerModel.Transform.rotation = rotation;
    }

    private void ReceivedDamage(float damage)
    {
        _playerModel.Health -= damage;
        _hudView.SetHealth(_playerModel.Health);
        _playerView.SetHealth(_playerModel.Health);
        
        if (_playerModel.Health <= 0.0f)
        {
            _respawnCoroutine = Respawn().ToObservable().Subscribe();
        }
    }

    private IEnumerator Respawn()
    {
        _playerModel.IsDead = true;
        _playerModel.Health = _playerModel.MaxHealth;
        
        _playerModel.CharacterController.enabled = !_playerModel.IsDead;
        _playerModel.Transform.position = new Vector3(25, 10, -25);

        _playerView.SetHealth(_playerModel.Health);
        _playerView.SetDead(_playerModel.IsDead);

        _hudView.SetHealth(_playerModel.Health);
        _hudView.SetDead(_playerModel.IsDead, _deathMessages[Random.Range(0, _deathMessages.Length)]);
        
        SwitchRenderers(!_playerModel.IsDead);

        var timer = _respawnTime;
        while (timer > 0)
        {
            timer -= _deltaTime;
            _hudView.SetTimer(timer);
            yield return new WaitForEndOfFrame();
        }

        _playerModel.IsDead = false;
        
        var (position, rotation) = GetRandomPosition();
        
        _playerModel.Transform.position = position;
        _playerModel.Transform.rotation = rotation;

        yield return new WaitForEndOfFrame();
        
        _playerModel.CharacterController.enabled = !_playerModel.IsDead;

        _playerView.SetDead(_playerModel.IsDead);

        _hudView.SetDead(_playerModel.IsDead, "");
        
        SwitchRenderers(!_playerModel.IsDead);
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

    private void SwitchRenderers(bool enabled)
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = enabled;
        }
    }

    public void Cleanup()
    {
        _respawnCoroutine?.Dispose();
        _playerView.OnReceivedDamage -= ReceivedDamage;
    }

    public void Execute(float deltaTime)
    {
        _deltaTime = deltaTime;
    }
}