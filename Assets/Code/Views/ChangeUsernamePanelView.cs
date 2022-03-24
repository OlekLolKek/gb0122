using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class ChangeUsernamePanelView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_Text _usernameErrorText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _backButton;

    private const string ERROR_TEXT = "The username should be between 3 and 25 characters long.";
    
    public event Action<string> OnConfirmButtonClicked = delegate { };
    public event Action OnBackButtonClicked = delegate { };

    private void Start()
    {
        _usernameInputField.onValueChanged.AddListener(InputFieldValueChanged);
        _usernameInputField.onSubmit.AddListener(InputFieldSubmit);
        _confirmButton.onClick.AddListener(ConfirmButtonClicked);
        _backButton.onClick.AddListener(BackButtonClicked);
        
        _usernameErrorText.text = string.Empty;
    }

    private void OnDestroy()
    {
        _usernameInputField.onValueChanged.RemoveListener(InputFieldValueChanged);
        _usernameInputField.onSubmit.RemoveListener(InputFieldSubmit);
        _confirmButton.onClick.RemoveListener(ConfirmButtonClicked);
        _backButton.onClick.RemoveListener(BackButtonClicked);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        
        _usernameInputField.text = string.Empty;
        _usernameErrorText.text = string.Empty;
        _usernameErrorText.text = string.Empty;
    }

    private void InputFieldValueChanged(string value)
    {
        _usernameErrorText.text = CheckUsernameEligible(value) ? string.Empty : ERROR_TEXT;
    }

    private void InputFieldSubmit(string value)
    {
        if (CheckUsernameEligible(value))
        {
            OnConfirmButtonClicked.Invoke(value);
        }
    }

    private bool CheckUsernameEligible(string username)
    {
        return username.Length >= 3 && username.Length <= 25;
    }

    private void ConfirmButtonClicked()
    {
        if (CheckUsernameEligible(_usernameInputField.text))
        {
            OnConfirmButtonClicked.Invoke(_usernameInputField.text);
        }
    }

    private void BackButtonClicked()
    {
        OnBackButtonClicked.Invoke();
    }
}