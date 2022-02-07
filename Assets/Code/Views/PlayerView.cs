using Photon.Pun;
using UnityEngine;


public sealed class PlayerView : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private GameObject _head;

    public CharacterController CharacterController => _characterController;
    public PhotonView PhotonView => _photonView;
    public GameObject GroundCheck => _groundCheck;
    public GameObject Head => _head;
}