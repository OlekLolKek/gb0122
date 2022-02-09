using UnityEngine;


public sealed class BarrelAttachmentView : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _muzzle;

    public AudioSource AudioSource => _audioSource;
    public GameObject Muzzle => _muzzle;
}