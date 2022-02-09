using UnityEngine;


[CreateAssetMenu(fileName = "AssaultRifleScopeData", menuName = "Data/Weapon/Attachment/AssaultRifleScope")]
public sealed class ScopeData : ScriptableObject, IScopeData
{
    [SerializeField] private ScopeView _prefab;
    public ScopeView Prefab => _prefab;
}