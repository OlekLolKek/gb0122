using UnityEngine;


public sealed class WeaponView : MonoBehaviour
{
    [SerializeField] private AudioSource _shotAudioSource;
    [SerializeField] private GameObject _scopeRail;
    [SerializeField] private GameObject _muzzle;

    public AudioSource ShotAudioSource => _shotAudioSource;
    public GameObject ScopeRail => _scopeRail;
    public GameObject Muzzle => _muzzle;
}