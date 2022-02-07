using UnityEngine;


public sealed class ScopeFactory : IFactory
{
    private readonly IScopeData _data;

    public ScopeFactory(IScopeData data)
    {
        _data = data;
    }
        
    public GameObject Create()
    {
        var scope = Object.Instantiate(_data.Prefab);

        return scope.gameObject;
    }
}