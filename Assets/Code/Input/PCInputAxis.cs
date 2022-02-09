using System;
using UnityEngine;


public sealed class PCInputAxis : IInputAxisChange
{
    public event Action<float> OnAxisChanged = delegate {  };
    
    private readonly string _axis;

    public PCInputAxis(string axis)
    {
        _axis = axis;
    }

    public void GetAxis()
    {
        OnAxisChanged.Invoke(Input.GetAxis(_axis));
    }
}