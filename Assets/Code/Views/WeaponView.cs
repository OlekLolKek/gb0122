using Photon.Pun;
using UnityEngine;


public sealed class WeaponView : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
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

    public void PlayShotAudio()
    {
        _photonView.RPC(nameof(ShotRPC), RpcTarget.All);
    }

    public void PlayReloadAudio()
    {
        _photonView.RPC(nameof(ReloadRPC), RpcTarget.All);
    }

    public void PlayEmptyClickAudio()
    {
        _photonView.RPC(nameof(EmptyClickRPC), RpcTarget.All);
    }

    [PunRPC]
    private void ShotRPC()
    {
        _shotAudioSource.Play();
    }
    
    [PunRPC]
    private void ReloadRPC()
    {
        _reloadAudioSource.Play();
    }
    
    [PunRPC]
    private void EmptyClickRPC()
    {
        _emptyClickAudioSource.Play();
    }
}