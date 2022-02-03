using System.Collections.Generic;


public sealed class Inputs : IExecutable
{
    private readonly List<IInputKeyHold> _holds;
    private readonly List<IInputKeyPress> _presses;
    private readonly List<IInputKeyRelease> _releases;
    private readonly List<IInputAxisChange> _axisChanges;
        
        
    public Inputs()
    {
        _presses = new List<IInputKeyPress>();
        _axisChanges = new List<IInputAxisChange>();
        _holds = new List<IInputKeyHold>();
        _releases = new List<IInputKeyRelease>();
    }

    internal void Add(IInput input)
    {
        if (input is IInputKeyHold hold)
            _holds.Add(hold);

        if (input is IInputKeyPress press)
            _presses.Add(press);

        if (input is IInputKeyRelease release)
            _releases.Add(release);

        if (input is IInputAxisChange axisChange)
            _axisChanges.Add(axisChange);
    }

    public void Execute(float deltaTime)
    {
        foreach (var hold in _holds)
            hold.GetKey();
        foreach (var press in _presses)
            press.GetKeyDown();
        foreach (var release in _releases)
            release.GetKeyUp();
        foreach (var axisChange in _axisChanges)
            axisChange.GetAxis();
    }
}