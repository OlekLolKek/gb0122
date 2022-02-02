using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class RoomAdminPanelView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _roomPrivacyButton;
    [SerializeField] private TMP_Text _privacyButtonText;

    public event Action OnStartButtonClicked = delegate { };
    public event Action<bool> OnPrivacyButtonClicked = delegate {  };
    
    private void Start()
    {
        _roomPrivacyButton.onClick.AddListener(PrivacyButtonClicked);
        _startButton.onClick.AddListener(StartButtonClicked);
    }

    private void OnEnable()
    {
        var isOpen = PhotonNetwork.CurrentRoom.IsOpen;
        _privacyButtonText.text = isOpen ? "Close room" : "Open room";
    }

    private void OnDestroy()
    {
        _roomPrivacyButton.onClick.RemoveListener(PrivacyButtonClicked);
        _startButton.onClick.RemoveListener(StartButtonClicked);
    }

    private void StartButtonClicked()
    {
        OnStartButtonClicked.Invoke();
    }

    private void PrivacyButtonClicked()
    {
        var isOpen = PhotonNetwork.CurrentRoom.IsOpen;
        _privacyButtonText.text = isOpen ? "Open room" : "Close room";
        OnPrivacyButtonClicked.Invoke(!isOpen);
    }
}