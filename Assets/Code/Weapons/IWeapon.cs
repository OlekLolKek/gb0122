using System;
using UnityEngine;


public interface IWeapon : IExecutable, ICleanable
{
    GameObject Instance { get; }
    void Fire();
    void AutoFire();
    void Reload();
    void Activate();
    void Deactivate();
    void Rotate(float mouseX, float mouseY, float deltaTime);
}