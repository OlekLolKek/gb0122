using System;
using UnityEngine;


public sealed class PCInputKeyDown : IInputKeyPress
{
    public event Action OnKeyPressed = delegate {  };
    
    private readonly KeyCode _keyCode;

    public PCInputKeyDown(KeyCode keyCode)
    {
        _keyCode = keyCode;
    }
        
    public void GetKeyDown()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            OnKeyPressed.Invoke();
        }
    }
}