using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class ChangeUsernamePanelView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _backButton;
    
    public event Action<string> OnConfirmButtonClicked = delegate {  };
    public event Action OnBackButtonClicked = delegate {  };

    private void Start()
    {
        _confirmButton.onClick.AddListener(ConfirmButtonClicked);
        _backButton.onClick.AddListener(BackButtonClicked);
    }

    private void OnDestroy()
    {
        _confirmButton.onClick.RemoveListener(ConfirmButtonClicked);
        _backButton.onClick.RemoveListener(BackButtonClicked);
    }

    private void ConfirmButtonClicked()
    {
        OnConfirmButtonClicked.Invoke(_usernameInputField.text);
    }

    private void BackButtonClicked()
    {
        OnBackButtonClicked.Invoke();
    }
}