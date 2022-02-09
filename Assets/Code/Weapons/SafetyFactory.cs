using UnityEngine;


public sealed class SafetyFactory : IFactory
{
    private readonly ISafetyData _data;
        
    public AudioSource AudioSource { get; private set; }

    public SafetyFactory(ISafetyData data)
    {
        _data = data;
    }
        
    public GameObject Create()
    {
        var safety = Object.Instantiate(_data.Prefab);
        AudioSource = safety.AudioSource;

        return safety.gameObject;
    }
}