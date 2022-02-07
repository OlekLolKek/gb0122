using UnityEngine;


public interface IScope
{
    GameObject Instance { get; }
    bool IsActive { get; set; }

    void Activate();
    void Deactivate();
}