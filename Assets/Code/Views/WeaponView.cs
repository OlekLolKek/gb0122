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
    [SerializeField] private int _otherPlayerLayer;

    private Renderer[] _renderers;
    private float _initialPitch;
    
    public GameObject Muzzle => _muzzle;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        _initialPitch = _shotAudioSource.pitch;
    }

    public void Start()
    {
        if (!_photonView.IsMine)
        {
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = _otherPlayerLayer;
            }
        }
    }

    public void PlayShotAudio(float pitchRandomness)
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC(nameof(ShotRPC), RpcTarget.All, pitchRandomness);
        }
        else
        {
            ShotRPC(pitchRandomness);
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
    private void ShotRPC(float pitchRandomness)
    {
        _shotAudioSource.pitch = _initialPitch + Random.Range(-pitchRandomness, pitchRandomness);
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