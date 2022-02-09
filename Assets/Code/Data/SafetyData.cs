using UnityEngine;


[CreateAssetMenu(fileName = "SafetyData", menuName = "Data/Weapon/Attachment/SafetyData")]
public sealed class SafetyData : ScriptableObject, ISafetyData
{
    [SerializeField] private SafetyView _prefab;
    public SafetyView Prefab => _prefab;
}