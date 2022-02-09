using System;


public interface IInputKeyPress : IInput
{
    event Action OnKeyPressed;
    void GetKeyDown();
}