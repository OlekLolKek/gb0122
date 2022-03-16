using System;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public sealed class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _deleteAccountButton;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _loadingColor;
    [SerializeField] private Color _failureColor;

    private string _username;
    private string _password;
    private string _email;

    private const string AUTH_KEY = "player-unique-id";

    private void Start()
    {
        _signInButton.onClick.AddListener(TryLogin);
        _deleteAccountButton.onClick.AddListener(DeleteAccount);
        CheckAccount();
    }

    private void CheckAccount()
    {
        if (PlayerPrefs.HasKey(AUTH_KEY))
        {
            _buttonText.text = "Sign in";
        }
        else
        {
            _buttonText.text = "Register";
        }
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
            OnLoginSuccess(result);
            if (needCreation)
            {
                CreateInitialUsername();
            }
            else
            {
                SceneManager.LoadScene("MainProfile");
            }
        }, OnLoginFail);
        
        _text.text = "Signing in...";
        _text.color = _loadingColor;
    }

    private void CreateInitialUsername()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = $"Player {Random.Range(1000, 10000)}" },
            result =>
            {
                SceneManager.LoadScene("MainProfile");
            }, Debug.LogError);
    }

    private void DeleteAccount()
    {
        PlayerPrefs.DeleteKey(AUTH_KEY);
        CheckAccount();
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
        }, OnLoginSuccess, Debug.LogError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        var message = "Successfully logged in PlayFab.";
        _text.text = message;
        _text.color = _successColor;
        Debug.Log(message);
    }

    private void OnLoginFail(PlayFabError error)
    {
        var message = "<color=red>Failed to log into PlayFab</color>!";
        _text.text = message;
        _text.color = _failureColor;
        Debug.LogError($"{message} {error}");
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveListener(TryLogin);
        _deleteAccountButton.onClick.RemoveListener(DeleteAccount);
    }
}
