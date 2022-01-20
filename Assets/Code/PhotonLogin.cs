using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public sealed class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _connectButton;
    [SerializeField] private Button _disconnectButton;
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _connectButton.onClick.AddListener(Connect);
        _disconnectButton.onClick.AddListener(Disconnect);
    }
    
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Disconnect()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Photon left room.");
        }
        else
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Photon disconnected.");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon Connected to master.");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Photon joined a room.");
    }
}