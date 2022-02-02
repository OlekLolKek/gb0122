using TMPro;
using UnityEngine;


public sealed class PlayerListPanelView : MonoBehaviour
{
    [SerializeField] private Transform _elementsRoot;
    [SerializeField] private TMP_Text _roomNameText;

    public Transform ElementsRoot => _elementsRoot;

    public void SetRoomName(string roomName)
    {
        _roomNameText.text = roomName;
    }
}