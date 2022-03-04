using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


public sealed class BotController : IInitialization, IExecutable, ICleanable
{
    private readonly DamageableUnitsManager _damageableUnitsManager;
    private readonly BotSpawnPointView[] _spawnPoints;
    private readonly GameObject _instance;
    private readonly BotView _botView;

    private readonly float _respawnTime;
    private readonly float _maxHealth;
    private readonly float _vision;
    private readonly float _damage;

    private IDamageable[] _players;
    private IDamageable _target;
    private IDisposable _respawnCoroutine;

    private BotState _state;
    private float _deltaTime;
    private float _health;
    private bool _isDead;

    public BotController(BotData botData, BotFactory botFactory, int id,
        BotSpawnPointView[] spawnPoints)
    {
        _respawnTime = botData.BotRespawnTime;
        _maxHealth = botData.BotHealth;
        _health = botData.BotHealth;
        _vision = botData.BotVision;
        _damage = botData.BotDamage;

        _instance = botFactory.Create();
        _botView = botFactory.BotView;

        _botView.SetId(id);
        _botView.SetHealth(_health);
        _botView.OnReceivedDamage += OnBotDamaged;
        _damageableUnitsManager = _botView.Manager;
        _damageableUnitsManager.OnPlayerAdded += GetPlayers;
        GetPlayers();

        _spawnPoints = spawnPoints;
    }

    public void Initialize()
    {
    }

    private void GetPlayers()
    {
        _players = _damageableUnitsManager.GetAllPlayers();
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

        if (!_isDead)
        {
            ProcessStates(deltaTime);
        }
    }

    private void ProcessStates(float deltaTime)
    {
        switch (_state)
        {
            case BotState.Idle:
                Idle();
                break;
            case BotState.Following:
                Following();
                break;
            case BotState.Attacking:
                Attacking();
                break;
            case BotState.Dead:
                break;
        }
    }

    private void Following()
    {
        if (_target == null ||
            (_target.Instance.transform.position - _instance.transform.position).sqrMagnitude >=
            Mathf.Pow(_vision, 2))
        {
            _state = BotState.Idle;
        }
        else
        {
            _botView.NavMeshAgent.SetDestination(_target.Instance.transform.position);
        }
    }

    private void Attacking()
    {
    }

    private void Idle()
    {
        if (_players == null)
        {
            return;
        }

        foreach (var player in _players)
        {
            if ((player.Instance.transform.position - _instance.transform.position).sqrMagnitude <
                Mathf.Pow(_vision, 2))
            {
                _target = player;
                _state = BotState.Following;
                return;
            }
        }
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
        _state = BotState.Dead;
        _botView.SetDead(true);
        _botView.NavMeshAgent.enabled = false;

        var (position, rotation) = GetRandomPosition();

        var transform = _botView.transform;
        transform.position = position;
        transform.rotation = rotation;

        yield return new WaitForSeconds(_respawnTime);

        _state = BotState.Idle;
        _botView.SetDead(false);
        _botView.NavMeshAgent.enabled = true;
    }

    public void Cleanup()
    {
        _damageableUnitsManager.OnPlayerAdded -= GetPlayers;
        _botView.OnReceivedDamage -= OnBotDamaged;

        _respawnCoroutine?.Dispose();
    }
}