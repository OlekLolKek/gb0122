using Photon.Pun;
using UnityEngine;


public sealed class WeaponView : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private AudioSource _shotAudioSource;
    [SerializeField] private AudioSource _emptyClickAudioSource;
    [SerializeField] private AudioSource _reloadAudioSource;
    [SerializeField] private GameObject _muzzle;
    
    public GameObject Muzzle => _muzzle;

    public void PlayShotAudio()
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC(nameof(ShotRPC), RpcTarget.All);
        }
        else
        {
            ShotRPC();
        }
    }

    public void PlayReloadAudio()
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC(nameof(ReloadRPC), RpcTarget.All);
        }
        else
        {
            ReloadRPC();
        }
    }

    public void PlayEmptyClickAudio()
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC(nameof(EmptyClickRPC), RpcTarget.All);
        }
        else
        {
            EmptyClickRPC();
        }
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