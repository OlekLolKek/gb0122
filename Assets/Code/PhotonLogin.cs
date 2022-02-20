using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public sealed class PhotonLogin : MonoBehaviourPunCallbacks
{
    [Header("Managers")]
    [SerializeField] private PlayerListManager _playerListManager;
    
    [Header("Panels")]
    [SerializeField] private RoomAdminPanelView _roomAdminPanelView;
    [SerializeField] private RoomListPanelView _roomListPanelView;
    [SerializeField] private GameObject _inventoryPanel;

    [SerializeField] private Button _showRoomsButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _roomListPanelView.OnJoinRoomButtonClicked += JoinSelectedRoom;
        _roomListPanelView.OnCreateRoomButtonClicked += CreateRoom;
        _showRoomsButton.onClick.AddListener(OnShowRoomsButtonClicked);

        _playerListManager.OnKickPlayer += KickPlayer;
    }

    private void OnDestroy()
    {
        _roomListPanelView.OnJoinRoomButtonClicked -= JoinSelectedRoom;
        _roomListPanelView.OnCreateRoomButtonClicked -= CreateRoom;
        _showRoomsButton.onClick.RemoveListener(OnShowRoomsButtonClicked);
        
        _playerListManager.OnKickPlayer -= KickPlayer;
    }

    private void Start()
    {
        Connect();
        PhotonNetwork.EnableCloseConnection = true;
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

    private void CreateRoom(string roomName, byte maxPlayers, bool isVisible)
    {
        if (string.IsNullOrWhiteSpace(roomName))
        {
            roomName = $"Room {Random.Range(0, 100000)}";
        }
        
        var roomOptions = new RoomOptions { IsVisible = isVisible, MaxPlayers = maxPlayers};
        
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private void JoinSelectedRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);

        _roomListPanelView.gameObject.SetActive(false);
    }

    private void OnShowRoomsButtonClicked()
    {
        _roomListPanelView.gameObject.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomListPanelView.SetRooms(roomList);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        Debug.Log("Photon Connected to master.");

        OnLeftRoom();
    }

    public override void OnCreatedRoom()
    {
        _roomListPanelView.gameObject.SetActive(false);
        _roomAdminPanelView.gameObject.SetActive(true);
        _roomAdminPanelView.OnPrivacyButtonClicked += SwitchRoomPrivacy;
        _roomAdminPanelView.OnStartButtonClicked += OnStartGameButtonClicked;
    }

    public override void OnLeftRoom()
    {
        _playerListManager.OnLeftRoom();

        if (_roomAdminPanelView == null || !_roomAdminPanelView.gameObject.activeSelf) return;
        _roomAdminPanelView.gameObject.SetActive(false);
        _roomAdminPanelView.OnPrivacyButtonClicked -= SwitchRoomPrivacy;
        _roomAdminPanelView.OnStartButtonClicked -= OnStartGameButtonClicked;
    }

    private void SwitchRoomPrivacy(bool open)
    {
        PhotonNetwork.CurrentRoom.IsOpen = open;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Create room failed! Code: {returnCode}, message: {message}");
    }

    public override void OnJoinedRoom()
    {
        _playerListManager.OnJoinedRoom();
        
        _inventoryPanel.SetActive(false);
    }

    private void KickPlayer(Player playerToKick)
    {
        PhotonNetwork.CloseConnection(playerToKick);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _playerListManager.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _playerListManager.OnPlayerLeftRoom(otherPlayer);
    }

    private void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Game");
    }
}