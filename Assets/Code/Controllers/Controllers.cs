using System.Collections.Generic;


public sealed class Controllers : IInitialization, IExecutable, ILateExecutable, ICleanable
{
    private readonly List<IInitialization> _initializationList;
    private readonly List<IExecutable> _executableList;
    private readonly List<ILateExecutable> _lateExecutableList;
    private readonly List<ICleanable> _cleanableList;
        
        
    public Controllers()
    {
        _initializationList = new List<IInitialization>();
        _executableList = new List<IExecutable>();
        _lateExecutableList = new List<ILateExecutable>();
        _cleanableList = new List<ICleanable>();
    }

    internal Controllers Add(IControllable controller)
    {
        if (controller is IInitialization init)
            _initializationList.Add(init);

        if (controller is IExecutable execute)
            _executableList.Add(execute);

        if (controller is ILateExecutable lateExecutable)
            _lateExecutableList.Add(lateExecutable);

        if (controller is ICleanable cleanup)
            _cleanableList.Add(cleanup);

        return this;
    }

    public void Initialize()
    {
        foreach (var controller in _initializationList)
            controller.Initialize();
    }

    public void Execute(float deltaTime)
    {
        foreach (var controller in _executableList)
            controller.Execute(deltaTime);
    }

    public void LateExecute(float deltaTime)
    {
        foreach (var controller in _lateExecutableList)
            controller.LateExecute(deltaTime);
    }

    public void Cleanup()
    {
        foreach (var controller in _cleanableList)
            controller.Cleanup();
    }
}