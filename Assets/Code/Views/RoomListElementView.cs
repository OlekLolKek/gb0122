using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class RoomListElementView : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Text _playersCountText;
    [SerializeField] private TMP_Text _mapNameText;
    [SerializeField] private Button _joinButton;

    private RoomInfo _roomInfo;

    public event Action<RoomInfo> OnJoinRoomButtonClicked = delegate {  };
    
    private void Start()
    {
        _joinButton.onClick.AddListener(JoinRoomButtonClicked);
    }

    private void JoinRoomButtonClicked()
    {
        OnJoinRoomButtonClicked.Invoke(_roomInfo);
    }

    public void SetRoom(RoomInfo room)
    {
        _roomInfo = room;
        _roomNameText.text = _roomInfo.Name;
        _playersCountText.text = $"{_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers}";
    }

    private void OnDestroy()
    {
        _joinButton.onClick.RemoveListener(JoinRoomButtonClicked);
    }
}