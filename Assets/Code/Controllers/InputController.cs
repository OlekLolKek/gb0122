﻿public sealed class InputController : IExecutable
{
    private readonly Inputs _inputs;

    public InputController(InputModel inputModel)
    {
        _inputs = new Inputs();
            
        _inputs.Add(inputModel.Horizontal);
        _inputs.Add(inputModel.Vertical);
        _inputs.Add(inputModel.MouseX);
        _inputs.Add(inputModel.MouseY);
        _inputs.Add(inputModel.StartCrouch);
        _inputs.Add(inputModel.StopCrouch);
        _inputs.Add(inputModel.Jump);
        _inputs.Add(inputModel.Weapon1);
        _inputs.Add(inputModel.Weapon2);
        _inputs.Add(inputModel.Weapon3);
        _inputs.Add(inputModel.SingleFire);
        _inputs.Add(inputModel.AutoFire);
        _inputs.Add(inputModel.Reload);
    }

    public void Execute(float deltaTime)
    {
        _inputs.Execute(deltaTime);
    }
}