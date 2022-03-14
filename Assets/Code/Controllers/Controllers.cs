using System.Collections.Generic;


public sealed class Controllers : IInitialization, IExecutable, ILateExecutable, IFixedExecutable, IMatchStateListener,
    ICleanable
{
    private readonly List<IInitialization> _initializationList = new List<IInitialization>();
    private readonly List<IExecutable> _executableList = new List<IExecutable>();
    private readonly List<ILateExecutable> _lateExecutableList = new List<ILateExecutable>();
    private readonly List<IFixedExecutable> _fixedExecutables = new List<IFixedExecutable>();
    private readonly List<IMatchStateListener> _startGameListeners = new List<IMatchStateListener>();
    private readonly List<ICleanable> _cleanableList = new List<ICleanable>();


    internal Controllers Add(IControllable controller)
    {
        if (controller is IInitialization init)
            _initializationList.Add(init);

        if (controller is IExecutable execute)
            _executableList.Add(execute);

        if (controller is ILateExecutable lateExecutable)
            _lateExecutableList.Add(lateExecutable);

        if (controller is IFixedExecutable fixedExecutable)
            _fixedExecutables.Add(fixedExecutable);

        if (controller is IMatchStateListener startGameListener)
            _startGameListeners.Add(startGameListener);

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

    public void FixedExecute()
    {
        foreach (var controller in _fixedExecutables)
            controller.FixedExecute();
    }

    public void ChangeMatchState(MatchState matchState)
    {
        foreach (var startGameListener in _startGameListeners)
            startGameListener.ChangeMatchState(matchState);
    }

    public void Cleanup()
    {
        foreach (var controller in _cleanableList)
            controller.Cleanup();
    }
}