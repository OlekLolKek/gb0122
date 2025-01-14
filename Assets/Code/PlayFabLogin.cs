using System;
using System.Collections.Generic;
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
    [SerializeField] private Button _quitButton;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _loadingColor;
    [SerializeField] private Color _failureColor;

    private string _username;

    private const string AUTH_KEY = "player-unique-id";

    private void Start()
    {
        _signInButton.onClick.AddListener(TryLogin);
        _deleteAccountButton.onClick.AddListener(DeleteAccount);
        _quitButton.onClick.AddListener(Quit);
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

    public void UpdateUsername(string username)
    {
        _username = username;
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

    private void OnLoginSuccess(LoginResult result)
    {
        var message = "Successfully logged in PlayFab.";
        _text.text = message;
        _text.color = _successColor;
        TryGetData();
    }

    private void TryGetData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = new List<string>
                    { Constants.SCORE_DATA_ID, Constants.TOTAL_SCORE_DATA_ID, Constants.LEVEL_DATA_ID }
            }, GotUserData, Debug.LogError
        );
    }

    private void GotUserData(GetUserDataResult result)
    {
        var dataToWrite = new Dictionary<string, string>();
        var data = result.Data;

        if (!data.ContainsKey(Constants.SCORE_DATA_ID))
        {
            dataToWrite.Add(Constants.SCORE_DATA_ID, 0.ToString());
        }
        
        if (!data.ContainsKey(Constants.TOTAL_SCORE_DATA_ID))
        {
            dataToWrite.Add(Constants.TOTAL_SCORE_DATA_ID, 0.ToString());
        }
        
        if (!data.ContainsKey(Constants.LEVEL_DATA_ID))
        {
            dataToWrite.Add(Constants.LEVEL_DATA_ID, 0.ToString());
        }

        if (dataToWrite.Count > 0)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
                {
                    Data = dataToWrite
                }, dataResult =>
                {
                    Debug.Log($"Created initial user data");
                },
                Debug.LogError);
        }
    }

    private void OnLoginFail(PlayFabError error)
    {
        var message = "<color=red>Failed to log into PlayFab</color>!";
        _text.text = message;
        _text.color = _failureColor;
        Debug.LogError($"{message} {error}");
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveListener(TryLogin);
        _deleteAccountButton.onClick.RemoveListener(DeleteAccount);
        _quitButton.onClick.RemoveListener(Quit);
    }
}
