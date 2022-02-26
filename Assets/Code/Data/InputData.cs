using UnityEngine;


[CreateAssetMenu(fileName = "InputData", menuName = "Data/InputData")]
public sealed class InputData : ScriptableObject, IData
{
    [SerializeField] private KeyCode _crouch;
    [SerializeField] private KeyCode _jump;
    [SerializeField] private KeyCode _weapon1;
    [SerializeField] private KeyCode _weapon2;
    [SerializeField] private KeyCode _weapon3;
    [SerializeField] private KeyCode _singleFire;
    [SerializeField] private KeyCode _autoFire;
    [SerializeField] private KeyCode _reload;

    public KeyCode Crouch => _crouch;
    public KeyCode Jump => _jump;
    public KeyCode Weapon1 => _weapon1;
    public KeyCode Weapon2 => _weapon2;
    public KeyCode Weapon3 => _weapon3;
    public KeyCode SingleFire => _singleFire;
    public KeyCode AutoFire => _autoFire;
    public KeyCode Reload => _reload;
}