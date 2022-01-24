using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public sealed class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _loadingColor;
    [SerializeField] private Color _failureColor;

    private string _username;
    private string _password;
    private string _email;

    private const string AUTH_KEY = "player-unique-id";

    private void Start()
    {
        _button.onClick.AddListener(TryLogin);
    }

    public void UpdatePassword(string password)
    {
        _password = password;
    }

    public void UpdateUsername(string username)
    {
        _username = username;
    }

    public void UpdateEmail(string email)
    {
        _email = email;
    }

    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _email,
            Password = _password,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log("Success");
        }, error =>
        {
            Debug.LogError($"Error: {error}");
        });
    }
    
    public void Login()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password,
        }, result =>
        {
            Debug.Log($"Success: {_username}");
        }, error =>
        {
            Debug.LogError($"Error: {error}");
        });
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

        var needCreation = !PlayerPrefs.HasKey(AUTH_KEY);
        Debug.Log($"needCreation: {needCreation}");
        var id = PlayerPrefs.GetString(AUTH_KEY, Guid.NewGuid().ToString());
        Debug.Log($"id: {id}");
        var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = needCreation };
        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            var message = "PlayFab Success";
            _text.text = message;
            _text.color = _successColor;
            PlayerPrefs.SetString(AUTH_KEY, id);
            Debug.Log(message);
            SceneManager.LoadScene("MainProfile");
        }, OnLoginFail);
        _text.text = "Signing in...";
        _text.color = _loadingColor;
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
