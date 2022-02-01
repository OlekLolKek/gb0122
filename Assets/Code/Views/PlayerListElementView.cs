using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class PlayerListElementView : MonoBehaviour
{
    [SerializeField] private TMP_Text _usernameText;
    [SerializeField] private Button _kickButton;

    private int _roomOwnerID;
    
    public event Action OnKickButtonClicked = delegate {  };

    private void Start()
    {
        _kickButton.onClick.AddListener(KickButtonClicked);
    }

    private void OnDestroy()
    {
        _kickButton.onClick.RemoveListener(KickButtonClicked);
    }

    private void KickButtonClicked()
    {
        OnKickButtonClicked.Invoke();
    }

    public void Initialize(int roomOwnerID, string playerUsername)
    {
        _roomOwnerID = roomOwnerID;

        _usernameText.text = playerUsername;
    }
}