using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public sealed class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayersElement _elementPrefab;
    [SerializeField] private GameObject _playerListPanel;
    [SerializeField] private GameObject _createRoomPanel;
    [SerializeField] private GameObject _inventoryPanel;

    private string _roomName;
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
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

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon Connected to master.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Create room failed! Code: {returnCode}, message: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom success! {_roomName}");
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"Added player {player.NickName} | {player.UserId}");

            var newElement = Instantiate(_elementPrefab, _playerListPanel.transform);
            newElement.gameObject.SetActive(true);
            newElement.SetPlayer(player);
        }

        _createRoomPanel.SetActive(false);
        _inventoryPanel.SetActive(false);
        _playerListPanel.SetActive(true);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate success");
    }

    public void UpdateRoomName(string roomName)
    {
        _roomName = roomName;
    }

    public void OnCreateRoomButtonClicked()
    {
        PhotonNetwork.CreateRoom(string.IsNullOrWhiteSpace(_roomName) ? $"Room {Random.Range(0, 10000)}" : _roomName);
    }
}