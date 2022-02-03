using UnityEngine;


public sealed class CursorController : IInitialization
{
    public void Initialize()
    {
        Lock();
    }

    private void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}