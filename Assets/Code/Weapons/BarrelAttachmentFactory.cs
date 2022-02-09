using UnityEngine;


public sealed class BarrelAttachmentFactory : IFactory
{
    private readonly IBarrelAttachmentData _data;

    public Transform BarrelTransform { get; private set; }
    public AudioSource AudioSource { get; private set; }
    

    public BarrelAttachmentFactory(IBarrelAttachmentData data)
    {
        _data = data;
    }
        
    public GameObject Create()
    {
        var attachment = Object.Instantiate(_data.Prefab);

        BarrelTransform = attachment.Muzzle.transform;

        AudioSource = attachment.AudioSource;
        
        return attachment.gameObject;
    }
}