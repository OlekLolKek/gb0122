using System;
using UnityEngine;


public interface IWeapon : IExecutable
{
    GameObject Instance { get; }
    void Fire();
    void Activate();
    void Deactivate();
    void Rotate(float mouseX, float mouseY, float deltaTime);
    void SetModdedValues(Transform barrel, AudioSource audioSource);
    void SetDefaultValues();
}