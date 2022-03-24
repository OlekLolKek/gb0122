using Photon.Pun;
using UnityEngine;


public sealed class WeaponView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shotParticles;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private AudioSource _shotAudioSource;
    [SerializeField] private AudioSource _emptyClickAudioSource;
    [SerializeField] private AudioSource _reloadAudioSource;
    [SerializeField] private GameObject _muzzle;

    private Renderer[] _renderers;
    
    public GameObject Muzzle => _muzzle;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetLayer(int layer)
    {
        if (!_photonView.IsMine)
        {
            foreach (var child in GetComponentsInChildren<GameObject>())
            {
                child.layer = layer;
            }
        }
    }

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

    public void EnableRenderers(bool enable)
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC(nameof(EnableRenderersRpc), RpcTarget.All, enable);
        }
        else
        {
            EnableRenderersRpc(enable);
        }
    }

    [PunRPC]
    private void ShotRPC()
    {
        _shotAudioSource.Play();
        _shotParticles.Play();
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

    [PunRPC]
    private void EnableRenderersRpc(bool enable)
    {
        foreach (var renderer1 in _renderers)
        {
            renderer1.enabled = enable;
        }
    }
}