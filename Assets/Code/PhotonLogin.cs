using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _connectButton;

    private TMP_Text _buttonText;
    private Image _buttonImage;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _connectButton.onClick.AddListener(Connect);
        _buttonText = _connectButton.GetComponentInChildren<TMP_Text>();
        _buttonImage = _connectButton.GetComponentInChildren<Image>();
        SwitchButton(ConnectionState.Disconnected);
    }

    private void OnDestroy()
    {
        _connectButton.onClick.RemoveAllListeners();
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

        SwitchButton(ConnectionState.Connecting);
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

        SwitchButton(ConnectionState.Disconnected);
    }

    private void SwitchButton(ConnectionState state)
    {
        switch (state)
        {
            case ConnectionState.Disconnected:
                _connectButton.onClick.RemoveAllListeners();
                _connectButton.onClick.AddListener(Connect);
                _connectButton.interactable = true;
                _buttonImage.color = Color.cyan;
                _buttonText.text = "Connect";
                break;
            case ConnectionState.Connected:
                _connectButton.onClick.RemoveAllListeners();
                _connectButton.onClick.AddListener(Disconnect);
                _connectButton.interactable = true;
                _buttonImage.color = Color.red;
                _buttonText.text = "Disconnect";
                break;
            case ConnectionState.Connecting:
                _connectButton.onClick.RemoveAllListeners();
                _connectButton.interactable = false;
                _buttonImage.color = Color.yellow;
                _buttonText.text = "Connecting...";
                break;
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon Connected to master.");
        SwitchButton(ConnectionState.Connected);
    }
}