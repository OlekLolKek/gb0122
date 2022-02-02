using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class PlayerListElementView : MonoBehaviour
{
    [SerializeField] private TMP_Text _usernameText;
    [SerializeField] private Button _kickButton;

    public int PlayerActorNumber { get; private set; }

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

    public void Initialize(int playerActorNumber, string playerUsername)
    {
        PlayerActorNumber = playerActorNumber;

        _usernameText.text = playerUsername;
    }
}