using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public sealed class PhotonLogin : MonoBehaviourPunCallbacks
{
    [Header("Prefabs")]
    [SerializeField] private PlayerListElementView _playerListElementPrefab;

    [Header("Panels")]
    [SerializeField] private PlayerListPanelView _playerListPanelView;
    [SerializeField] private RoomAdminPanelView _roomAdminPanelView;
    [SerializeField] private RoomListPanelView _roomListPanelView;
    [SerializeField] private Transform _playerListRoot;
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

        _roomListPanelView.gameObject.SetActive(false);
    }

    private void JoinSelectedRoom(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);

        _roomListPanelView.gameObject.SetActive(false);
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

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        _roomAdminPanelView.gameObject.SetActive(true);
        _roomAdminPanelView.OnPrivacyButtonClicked += SwitchRoomPrivacy;
        _roomAdminPanelView.OnStartButtonClicked += OnStartGameButtonClicked;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (_roomAdminPanelView == null || !_roomAdminPanelView.gameObject.activeSelf) return;
        _roomAdminPanelView.gameObject.SetActive(false);
        _roomAdminPanelView.OnPrivacyButtonClicked -= SwitchRoomPrivacy;
        _roomAdminPanelView.OnStartButtonClicked -= OnStartGameButtonClicked;
    }

    private void SwitchRoomPrivacy(bool open)
    {
        PhotonNetwork.CurrentRoom.IsOpen = open;
        Debug.Log($"Room is open set to {open}");
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

            var newElement = Instantiate(_playerListElementPrefab, _playerListPanelView.ElementsRoot);
            newElement.gameObject.SetActive(true);
            newElement.Initialize(player.ActorNumber, player.NickName);
        }
        
        _inventoryPanel.SetActive(false);
        
        _playerListPanelView.SetRoomName(PhotonNetwork.CurrentRoom.Name);
        _playerListPanelView.gameObject.SetActive(true);
    }

    private void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Game");
    }
}