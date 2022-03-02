using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public sealed class DamageableUnitsManager : MonoBehaviour
{
    private readonly Dictionary<int, IDamageable> _damageables = new Dictionary<int, IDamageable>();

    public void Register(int id, IDamageable damageable)
    {
        _damageables.Add(id, damageable);
    }

    [CanBeNull]
    public IDamageable GetDamageable(int id)
    {
        return _damageables.TryGetValue(id, out var damageable) ? damageable : null;
    }
}