using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public sealed class PlayerListManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private PlayerListElementView _playerListElementPrefab;

    [Header("Panels")]
    [SerializeField] private PlayerListPanelView _playerListPanelView;
    
    private readonly List<PlayerListElementView> _playerElements = new List<PlayerListElementView>();
    
    public event Action<Player> OnKickPlayer = delegate {  }; 

    private void OnDestroy()
    {
        foreach (var element in _playerElements)
        {
            element.OnKickButtonClicked -= KickPlayer;
        }
    }

    private void KickPlayer(Player playerToKick)
    {
        OnKickPlayer.Invoke(playerToKick);
    }

    public void OnLeftRoom()
    {
        foreach (var element in _playerElements)
        {
            if (element != null)
            {
                element.OnKickButtonClicked -= KickPlayer;
                Destroy(element.gameObject);
            }
        }
        
        _playerListPanelView.gameObject.SetActive(false);
    }
    
    private void CreatePlayerElement(Player newPlayer)
    {
        var newElement = Instantiate(_playerListElementPrefab, _playerListPanelView.ElementsRoot);
        newElement.gameObject.SetActive(true);
        newElement.Initialize(newPlayer, newPlayer.NickName);
        newElement.OnKickButtonClicked += KickPlayer;
        _playerElements.Add(newElement);
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        foreach (var element in _playerElements)
        {
            element.CheckNewMaster(newMasterClient);
        }
    }

    public void OnJoinedRoom()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            CreatePlayerElement(player);
        }
        
        _playerListPanelView.SetRoomName(PhotonNetwork.CurrentRoom.Name);
        _playerListPanelView.gameObject.SetActive(true);
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreatePlayerElement(newPlayer);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        var elementToDelete = _playerElements.FirstOrDefault(element => element.PlayerActorNumber == otherPlayer.ActorNumber);
        if (elementToDelete)
        {
            Destroy(elementToDelete.gameObject);
            elementToDelete.OnKickButtonClicked -= KickPlayer;
        }
    }
}