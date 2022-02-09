using System;
using UnityEngine;


public sealed class PCInputKeyUp : IInputKeyRelease
{
    public event Action OnKeyReleased = delegate {  };
    
    private readonly KeyCode _keyCode;

    public PCInputKeyUp(KeyCode keyCode)
    {
        _keyCode = keyCode;
    }
        
    public void GetKeyUp()
    {
        if (Input.GetKeyUp(_keyCode))
        {
            OnKeyReleased.Invoke();
        }
    }
}