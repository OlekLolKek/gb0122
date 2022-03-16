using System;
using System.Collections;
using Photon.Pun;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public sealed class BotController : IExecutable, IMatchStateListener, ICleanable
{
    private readonly DamageableUnitsManager _damageableUnitsManager;
    private readonly BotSpawnPointView[] _spawnPoints;
    private readonly TracerFactory _tracerFactory;
    private readonly GameObject _instance;
    private readonly LayerMask _hitMask;
    private readonly BotView _botView;

    private readonly float _attackSpreadMultiplier;
    private readonly float _tracerFadeMultiplier;
    private readonly float _maxAttackCooldown;
    private readonly float _minAttackCooldown;
    private readonly float _idlePositionRange;
    private readonly float _idleTimeRandom;
    private readonly float _idleDuration;
    private readonly float _respawnTime;
    private readonly float _attackRange;
    private readonly float _maxHealth;
    private readonly float _vision;
    private readonly float _damage;
    private readonly int _scoreForKill;

    private IDamageable[] _players;
    private IDamageable _target;
    private IDisposable _respawnCoroutine;
    private MatchState _matchState;

    private BotState _state = BotState.Idle;
    private bool _isReadyToShoot = true;
    private bool _isReadyToWalk;
    private float _idleTimer;
    private float _deltaTime;
    private float _health;
    
    private const float COLOR_FADE_MULTIPLIER = 15.0f;

    public BotController(BotData botData, BotFactory botFactory, int id,
        BotSpawnPointView[] spawnPoints, IWeaponData weaponData)
    {
        _attackSpreadMultiplier = botData.BotAttackSpreadMultiplier;
        _maxAttackCooldown = botData.BotMaxAttackCooldown;
        _minAttackCooldown = botData.BotMinAttackCooldown;
        _idlePositionRange = botData.BotIdlePositionRange;
        _idleTimeRandom = botData.BotIdleTimeRandom;
        _idleDuration = botData.BotIdleDuration;
        _respawnTime = botData.BotRespawnTime;
        _attackRange = botData.BotAttackRange;
        _maxHealth = botData.BotHealth;
        _health = botData.BotHealth;
        _vision = botData.BotVision;
        _damage = botData.BotDamage;
        _scoreForKill = botData.ScoreForKill;

        _instance = botFactory.Create();
        _botView = botFactory.BotView;

        _botView.SetId(id);
        _botView.SetHealth(_health);
        _botView.OnReceivedDamage += OnBotDamaged;
        _damageableUnitsManager = _botView.DamageableUnitsManager;
        _damageableUnitsManager.OnPlayerListChanged += GetPlayers;
        GetPlayers();

        _spawnPoints = spawnPoints;
        
        _tracerFactory = new TracerFactory(weaponData);
        _tracerFadeMultiplier = weaponData.TracerFadeMultiplier;
        _hitMask = botData.WeaponHitMask;
        _idleTimer = Random.Range(_idleDuration - _idleTimeRandom,
            _idleDuration + _idleTimeRandom);
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
        if (!_botView)
        {
            return;
        }
        
        
        _deltaTime = deltaTime;
        ProcessStates(deltaTime);
    }

    private void ProcessStates(float deltaTime)
    {
        if (_matchState != MatchState.MatchProcess)
        {
            _botView.NavMeshAgent.velocity = Vector3.zero;
            return;
        }

        
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

    private void Idle()
    {
        if (_players == null)
        {
            return;
        }
        
        if (_target != null)
        {
            _state = BotState.Following;
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

        if (!_isReadyToWalk)
        {
            _idleTimer -= _deltaTime;
            if (_idleTimer <= 0.0f)
            {
                _isReadyToWalk = true;
            }
        }


        if (_isReadyToWalk)
        {
            Vector3 direction = Random.insideUnitCircle * _idlePositionRange;
            (direction.y, direction.z) = (direction.z, direction.y);
            if (NavMesh.SamplePosition(direction, out var hit, _idlePositionRange, _hitMask))
            {
                _isReadyToWalk = false;
                _idleTimer = Random.Range(_idleDuration - _idleTimeRandom,
                    _idleDuration + _idleTimeRandom);
                _botView.NavMeshAgent.SetDestination(hit.position);
            }
        }
    }

    private void Following()
    {
        if (_target == null || _target.IsDead)
        {
            _state = BotState.Idle;
            _target = null;
        }
        else
        {
            if ((_target.Instance.transform.position - _instance.transform.position).sqrMagnitude <=
                Mathf.Pow(_attackRange, 2))
            {
                _state = BotState.Attacking;
            }
            else
            {
                _botView.NavMeshAgent.SetDestination(_target.Instance.transform.position);
            }
        }
    }

    private void Attacking()
    {
        if (_target == null || _target.IsDead ||
            (_target.Instance.transform.position - _instance.transform.position).sqrMagnitude >=
            Mathf.Pow(_vision, 2))
        {
            _state = BotState.Idle;
            _target = null;
        }
        else
        {
            if ((_target.Instance.transform.position - _instance.transform.position).sqrMagnitude >=
                Mathf.Pow(_attackRange, 2))
            {
                _state = BotState.Following;
            }
            else
            {
                _botView.NavMeshAgent.SetDestination(_botView.transform.position);
                var transform = _botView.transform;
                transform.LookAt(_target.Instance.transform);
                var rotation = transform.eulerAngles;
                rotation.x = 0.0f;
                rotation.z = 0.0f;
                transform.eulerAngles = rotation;

                if (_isReadyToShoot)
                {
                    var line = CreateTracer();

                    _botView.WeaponView.PlayShotAudio();

                    TweenLineWidth(line).ToObservable().Subscribe();
                    StartShootCooldown().ToObservable().Subscribe();
                }
            }
        }
    }
    
    private IEnumerator StartShootCooldown()
    {
        _isReadyToShoot = false;
        
        var timer = 0.0f;
        var cooldown = Random.Range(_minAttackCooldown, _maxAttackCooldown);
        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _isReadyToShoot = true;
    }

    private LineRenderer CreateTracer()
    {
        _tracerFactory.Create();
        var line = _tracerFactory.LineRenderer;

        var origin = _botView.WeaponView.Muzzle.transform.position;
        line.SetPosition(0, origin);
        var target = _target.Instance.transform.position + Random.insideUnitSphere * _attackSpreadMultiplier;
        var direction = target - origin;

        var ray = new Ray(origin, direction);
        
        if (Physics.Raycast(ray, out var hit, _attackRange, _hitMask))
        {
            line.SetPosition(1, hit.point);
            TryDamage(hit);
        }
        else
        {
            line.SetPosition(1, target);
        }

        return line;
    }
    
    private void TryDamage(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out IDamageable enemy))
        {
            if (enemy.CheckIfMine())
            {
                enemy.Damage(_damage, _botView);
            }
            else
            {
                _botView.SendIdToDamage(enemy.ID, _damage);
            }
        }
    }
    
    private IEnumerator TweenLineWidth(LineRenderer line)
    {
        while (line.endWidth > 0)
        {
            var endWidth = line.endWidth;
            endWidth -= _deltaTime * _tracerFadeMultiplier;
            line.endWidth = endWidth;
            line.startWidth = endWidth;

            var color = line.material.color;
            color.a -= _deltaTime * _tracerFadeMultiplier * COLOR_FADE_MULTIPLIER;
            line.material.color = color;

            yield return 0;
        }

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Destroy(line.gameObject);
        else
            Object.Destroy(line.gameObject);
    }

    private void OnBotDamaged(float damage, IDamageable sender)
    {
        _health -= damage;
        _botView.SetHealth(_health);

        if (_health <= 0.0f)
        {
            _respawnCoroutine = Respawn().ToObservable().Subscribe();
            _botView.PhotonView.RPC(nameof(_botView.AddStats), RpcTarget.All,
                sender.ID, 1, 0, _scoreForKill);
        }
        else
        {
            _target ??= sender;
        }
    }

    private IEnumerator Respawn()
    {
        _target = null;
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

    public void ChangeMatchState(MatchState matchState)
    {
        _matchState = matchState;
    }

    public void Cleanup()
    {
        _damageableUnitsManager.OnPlayerListChanged -= GetPlayers;
        _botView.OnReceivedDamage -= OnBotDamaged;

        _respawnCoroutine?.Dispose();
    }
}