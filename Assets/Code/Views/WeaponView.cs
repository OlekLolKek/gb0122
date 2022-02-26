using UnityEngine;


public sealed class WeaponView : MonoBehaviour
{
    [SerializeField] private AudioSource _shotAudioSource;
    [SerializeField] private AudioSource _emptyClickAudioSource;
    [SerializeField] private AudioSource _reloadAudioSource;
    [SerializeField] private GameObject _scopeRail;
    [SerializeField] private GameObject _muzzle;

    public AudioSource ShotAudioSource => _shotAudioSource;
    public AudioSource EmptyClickAudioSource => _emptyClickAudioSource;
    public AudioSource ReloadAudioSource => _reloadAudioSource;
    public GameObject ScopeRail => _scopeRail;
    public GameObject Muzzle => _muzzle;
}