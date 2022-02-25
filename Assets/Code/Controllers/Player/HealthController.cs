using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


public sealed class HealthController : IExecutable, ICleanable
{
    private readonly PlayerModel _playerModel;
    private readonly PlayerView _playerView;
    private readonly Renderer[] _renderers;
    private readonly HudView _hudView;
    private readonly float _maxHealth;
    private readonly float _respawnTime;
    private readonly string[] _deathMessages;

    private float _health;

    private IDisposable _respawnCoroutine;
    private float _deltaTime;

    public HealthController(PlayerModel playerModel, PlayerData playerData,
        HudView hudView)
    {
        _playerModel = playerModel;
        _playerView = playerModel.PlayerView;
        _maxHealth = playerData.MaxHealth;
        _health = playerData.StartHealth;
        _respawnTime = playerData.RespawnTime;
        _deathMessages = playerData.DeathMessages;
        _hudView = hudView;

        _playerView.OnReceivedDamage += ReceivedDamage;
        _hudView.SetHealth(_health);
        _playerView.SetHealth(_health);

        _renderers = _playerModel.Transform.GetComponentsInChildren<Renderer>();
    }

    private void ReceivedDamage(float damage)
    {
        _health -= damage;
        _hudView.SetHealth(_health);
        _playerView.SetHealth(_health);
        
        if (_health <= 0.0f)
        {
            _respawnCoroutine = Respawn().ToObservable().Subscribe();
        }
    }

    private IEnumerator Respawn()
    {
        _health = _maxHealth;
        _hudView.SetHealth(_health);
        _playerView.SetHealth(_health);
        _playerModel.CharacterController.enabled = false;
        _playerModel.Transform.position = new Vector3(25, 10, -25);
        _hudView.SetDead(true, _deathMessages[Random.Range(0, _deathMessages.Length)]);
        SwitchRenderers(false);

        var timer = _respawnTime;
        while (timer > 0)
        {
            timer -= _deltaTime;
            _hudView.SetTimer(timer);
            yield return new WaitForEndOfFrame();
        }
        
        SwitchRenderers(true);
        _hudView.SetDead(false, "");
        _playerModel.CharacterController.enabled = true;
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