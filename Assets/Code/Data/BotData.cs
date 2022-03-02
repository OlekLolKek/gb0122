using UnityEngine;


[CreateAssetMenu(menuName = "Data/" + nameof(BotData), fileName = nameof(BotData))]
public sealed class BotData : ScriptableObject, IData
{
    [SerializeField] private BotView _botPrefab;
    [SerializeField] private float _botRespawnTime;
    [SerializeField] private int _botHealth;
    [SerializeField] private int _botDamage;
    [SerializeField] private int _botAmount;

    public BotView BotPrefab => _botPrefab;
    public float BotRespawnTime => _botRespawnTime;
    public int BotHealth => _botHealth;
    public int BotDamage => _botDamage;
    public int BotAmount => _botAmount;
}