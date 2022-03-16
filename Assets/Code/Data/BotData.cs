using UnityEngine;


[CreateAssetMenu(menuName = "Data/" + nameof(BotData), fileName = nameof(BotData))]
public sealed class BotData : ScriptableObject, IData
{
    [SerializeField] private BotView _botPrefab;
    [SerializeField] private LayerMask _weaponHitMask;
    [SerializeField] private float _botAttackSpreadMultiplier;
    [SerializeField] private float _botMinAttackCooldown;
    [SerializeField] private float _botMaxAttackCooldown;
    [SerializeField] private float _botIdlePositionRange;
    [SerializeField] private float _botIdleTimeRandom;
    [SerializeField] private float _botIdleDuration;
    [SerializeField] private float _botRespawnTime;
    [SerializeField] private float _botAttackRange;
    [SerializeField] private float _botVision;
    [SerializeField] private int _botHealth;
    [SerializeField] private int _botDamage;
    [SerializeField] private int _botAmount;
    [SerializeField] private int _botScoreForKill;

    public BotView BotPrefab => _botPrefab;
    public LayerMask WeaponHitMask => _weaponHitMask;
    public float BotAttackSpreadMultiplier => _botAttackSpreadMultiplier;
    public float BotMaxAttackCooldown => _botMaxAttackCooldown;
    public float BotMinAttackCooldown => _botMinAttackCooldown;
    public float BotIdlePositionRange => _botIdlePositionRange;
    public float BotIdleTimeRandom => _botIdleTimeRandom;
    public float BotIdleDuration => _botIdleDuration;
    public float BotRespawnTime => _botRespawnTime;
    public float BotAttackRange => _botAttackRange;
    public float BotVision => _botVision;
    public int BotHealth => _botHealth;
    public int BotDamage => _botDamage;
    public int BotAmount => _botAmount;
    public int ScoreForKill => _botScoreForKill;
}