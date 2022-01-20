using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public sealed class PlayFabLogin : MonoBehaviour
{
    private void Start()
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
        Debug.Log("PlayFab Success!");
    }

    private void OnLoginFail(PlayFabError error)
    {
        Debug.LogError($"PlayFab Fail!: {error}");
    }
}
