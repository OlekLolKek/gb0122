using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public sealed class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomListPanelView _roomListPanelView;
    [SerializeField] private PlayersElement _elementPrefab;
    [SerializeField] private GameObject _playerListPanel;
    [SerializeField] private GameObject _inventoryPanel;

    [SerializeField] private Button _showRoomsButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _roomListPanelView.OnJoinRoomButtonClicked += JoinSelectedRoom;
        _roomListPanelView.OnCreateRoomButtonClicked += CreateRoom;
        _showRoomsButton.onClick.AddListener(OnShowRoomsButtonClicked);
    }

    private void OnDestroy()
    {
        _roomListPanelView.OnJoinRoomButtonClicked -= JoinSelectedRoom;
        _roomListPanelView.OnCreateRoomButtonClicked -= CreateRoom;
        _showRoomsButton.onClick.RemoveListener(OnShowRoomsButtonClicked);
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

    private void CreateRoom(string roomName, byte maxPlayers)
    {
        if (string.IsNullOrWhiteSpace(roomName))
        {
            roomName = $"Room {Random.Range(0, 100000)}";
        }
        
        PhotonNetwork.CreateRoom(roomName, new RoomOptions {MaxPlayers = maxPlayers});
        
        Debug.Log("Created room");
    }

    private void JoinSelectedRoom(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
    }

    private void OnShowRoomsButtonClicked()
    {
        _roomListPanelView.gameObject.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomListPanelView.SetRooms(roomList);
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
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"Added player {player.NickName} | {player.UserId}");

            var newElement = Instantiate(_elementPrefab, _playerListPanel.transform);
            newElement.gameObject.SetActive(true);
            newElement.SetPlayer(player);
        }
        
        _inventoryPanel.SetActive(false);
        _playerListPanel.SetActive(true);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Game");
    }
}