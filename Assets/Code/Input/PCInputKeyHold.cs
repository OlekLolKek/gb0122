using System;
using UnityEngine;


public sealed class PCInputKeyHold : IInputKeyHold
{
    public event Action<bool> OnKeyHeld = delegate {  };
        
    private readonly KeyCode _keyCode;

    public PCInputKeyHold(KeyCode keyCode)
    {
        _keyCode = keyCode;
    }
        
    public void GetKey()
    {
        OnKeyHeld.Invoke(Input.GetKey(_keyCode));
    }
}