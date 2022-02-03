using System;


public interface IInputKeyHold : IInput
{
    event Action<bool> OnKeyHeld;
    void GetKey();
}