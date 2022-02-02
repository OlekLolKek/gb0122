using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class TextToCopyView : MonoBehaviour
{
    [SerializeField] private TMP_Text _textToCopy;
    [SerializeField] private Button _copyRoomNameButton;
    
    private void Start()
    {
        _copyRoomNameButton.onClick.AddListener(CopyRoomName);
    }

    private void OnDestroy()
    {
        _copyRoomNameButton.onClick.RemoveListener(CopyRoomName);
    }

    private void CopyRoomName()
    {
        var textEditor = new TextEditor();
        textEditor.text = _textToCopy.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }
}