using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


public sealed class DamageableUnitsManager : MonoBehaviour
{
    private readonly Dictionary<int, IDamageable> _damageables = new Dictionary<int, IDamageable>();
    
    public event Action OnPlayerAdded = delegate {  };

    public void Register(int id, IDamageable damageable)
    {
        _damageables.Add(id, damageable);
        OnPlayerAdded.Invoke();
    }

    [CanBeNull]
    public IDamageable GetDamageable(int id)
    {
        return _damageables.TryGetValue(id, out var damageable) ? damageable : null;
    }

    public IDamageable[] GetAllPlayers()
    {
        var players = new List<IDamageable>();
        players.AddRange(_damageables.Values.Where(enemy => enemy is PlayerView));
        return players.ToArray();
    }
}