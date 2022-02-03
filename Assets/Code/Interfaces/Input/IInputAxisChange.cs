using System;


public interface IInputAxisChange : IInput
{
    event Action<float> OnAxisChanged;
    void GetAxis();
}