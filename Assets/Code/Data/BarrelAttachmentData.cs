using UnityEngine;


[CreateAssetMenu(fileName = "AssaultRifleSilencerData", menuName = "Data/Weapon/Attachment/AssaultRifleSilencer")]
public sealed class BarrelAttachmentData : ScriptableObject, IBarrelAttachmentData
{
    [SerializeField] private BarrelAttachmentView _prefab;

    public BarrelAttachmentView Prefab => _prefab;
}