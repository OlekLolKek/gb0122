using System;


public interface IInputKeyRelease : IInput
{
    event Action OnKeyReleased;
    void GetKeyUp();
}