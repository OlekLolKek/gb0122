using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failureColor;
    
    private void Start()
    {
        _button.onClick.AddListener(TryLogin);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(TryLogin);
    }

    private void TryLogin()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "70913";
            Debug.Log("Successfully set the title ID.");
        }

        var request = new LoginWithCustomIDRequest { CustomId = "lesson3", CreateAccount = false };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFail);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        var message = "PlayFab Success";
        _text.text = message;
        _text.color = _successColor;
        Debug.Log(message);
    }

    private void OnLoginFail(PlayFabError error)
    {
        var message = "PlayFab Fail!";
        _text.text = message;
        _text.color = _failureColor;
        Debug.LogError($"{message}: {error}");
    }
}
