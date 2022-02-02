using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class RoomListPanelView : MonoBehaviour
{
    [Header("Room list")]
    [SerializeField] private GameObject _roomListPanel;
    [SerializeField] private RoomListElementView _roomListElementPrefab;
    [SerializeField] private Button _backButton;
    [SerializeField] private Transform _elementsRoot;
    [SerializeField] private Button _createRoomPanelButton;

    [Header("Direct connect")]
    [SerializeField] private TMP_InputField _roomToConnectInputField;
    [SerializeField] private Button _connectDirectlyButton;

    [Header("Create room panel")]
    [SerializeField] private GameObject _createRoomPanel;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private Slider _maxPlayersSlider;
    [SerializeField] private TMP_Text _maxPlayersText;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _roomListPanelButton;
    [SerializeField] private Toggle _privateRoomToggle;

    private readonly List<RoomListElementView> _roomListElements = new List<RoomListElementView>();
    
    public event Action<string> OnJoinRoomButtonClicked = delegate {  };
    public event Action<string, byte, bool> OnCreateRoomButtonClicked = delegate {  };

    private void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        
        _createRoomPanelButton.onClick.AddListener(OnCreateRoomPanelButtonClicked);
        _roomListPanelButton.onClick.AddListener(OnRoomListPanelButtonClicked);
        _createRoomButton.onClick.AddListener(CreateRoomButtonClicked);
        _connectDirectlyButton.onClick.AddListener(ConnectDirectlyButtonClicked);

        _maxPlayersSlider.onValueChanged.AddListener(OnMaxPlayersSliderValueChanged);

        OnMaxPlayersSliderValueChanged(_maxPlayersSlider.value);
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        
        _createRoomPanelButton.onClick.RemoveListener(OnCreateRoomPanelButtonClicked);
        _roomListPanelButton.onClick.RemoveListener(OnRoomListPanelButtonClicked);
        _createRoomButton.onClick.RemoveListener(CreateRoomButtonClicked);
        _connectDirectlyButton.onClick.RemoveListener(ConnectDirectlyButtonClicked);

        _maxPlayersSlider.onValueChanged.RemoveListener(OnMaxPlayersSliderValueChanged);
    }

    private void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnCreateRoomPanelButtonClicked()
    {
        _roomListPanel.gameObject.SetActive(false);
        _createRoomPanel.gameObject.SetActive(true);
    }

    private void OnRoomListPanelButtonClicked()
    {
        _roomListPanel.gameObject.SetActive(true);
        _createRoomPanel.gameObject.SetActive(false);
    }

    private void OnMaxPlayersSliderValueChanged(float value)
    {
        _maxPlayersText.text = $"Max players: {value}";
    }

    public void SetRooms(List<RoomInfo> roomList)
    {
        var i = 0;
        for (; i < _roomListElements.Count && i < roomList.Count; ++i)
        {
            _roomListElements[i].SetRoom(roomList[i]);
        }

        if (_roomListElements.Count < roomList.Count)
        {
            for (var j = i; j < roomList.Count; ++j)
            {
                var newElement = Instantiate(_roomListElementPrefab, _elementsRoot);
                newElement.SetRoom(roomList[j]);
                _roomListElements.Add(newElement);
                newElement.OnJoinRoomButtonClicked += JoinRoomButtonClicked;
            }
        }
        else if (_roomListElements.Count > roomList.Count)
        {
            for (var j = i; j < _roomListElements.Count; ++j)
            {
                var roomListElement = _roomListElements[j];
                _roomListElements.Remove(roomListElement);
                roomListElement.OnJoinRoomButtonClicked -= JoinRoomButtonClicked;
                Destroy(roomListElement.gameObject);
            }
        }
    }

    private void JoinRoomButtonClicked(RoomInfo roomInfo)
    {
        OnJoinRoomButtonClicked.Invoke(roomInfo.Name);
    }

    private void ConnectDirectlyButtonClicked()
    {
        OnJoinRoomButtonClicked.Invoke(_roomToConnectInputField.text);
    }

    private void CreateRoomButtonClicked()
    {
        OnCreateRoomButtonClicked.Invoke(_roomNameInputField.text, (byte)_maxPlayersSlider.value, !_privateRoomToggle.isOn);
    }
}