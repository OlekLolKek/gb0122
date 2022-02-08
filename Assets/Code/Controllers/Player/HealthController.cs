using System;
using System.Collections;
using UniRx;
using UnityEngine;


public sealed class HealthController : ICleanable
{
    private readonly PlayerModel _playerModel;
    private readonly PlayerView _playerView;
    private readonly Renderer[] _renderers;
    private readonly float _maxHealth;

    private float _health;

    private IDisposable _respawnCoroutine;

    public HealthController(PlayerModel playerModel, PlayerData playerData)
    {
        _playerModel = playerModel;
        _playerView = playerModel.PlayerView;
        _maxHealth = playerData.MaxHealth;
        _health = playerData.StartHealth;

        _playerView.OnReceivedDamage += ReceivedDamage;
        _playerView.SetHealth(_health);
        Debug.Log($"Subscribed to onHealthChanged of {_playerView.gameObject.name}");

        _renderers = _playerModel.Transform.GetComponentsInChildren<Renderer>();
    }

    private void ReceivedDamage(float damage)
    {
        _health -= damage;
        _playerView.SetHealth(_health);
        Debug.Log($"Health changed to {_health}");
        if (_health <= 0.0f)
        {
            _respawnCoroutine = Respawn().ToObservable().Subscribe();
        }
    }

    private IEnumerator Respawn()
    {
        _health = _maxHealth;
        _playerView.SetHealth(_health);
        _playerModel.CharacterController.enabled = false;
        _playerModel.Transform.position = new Vector3(25, 10, -25);
        SwitchRenderers(false);
        
        yield return new WaitForSeconds(1);
        
        SwitchRenderers(true);
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
}