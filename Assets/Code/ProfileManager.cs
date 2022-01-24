using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;


public sealed class ProfileManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _idText;
    
    private void Start()
    {
        _idText.text = "Getting user info...";
        _idText.color = Color.yellow;
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), success =>
        {
            _idText.text = $"Welcome back, {success.AccountInfo.Username}\n{GetPlayerInfo(success.AccountInfo)}";
            _idText.color = Color.white;
        }, error =>
        {
            _idText.text = $"Something went wrong: {error}";
        });
    }

    private string GetPlayerInfo(UserAccountInfo info)
    {
        return $"Account created on {info.Created}\nCustom id: {info.CustomIdInfo.CustomId}\nPlayFab id: {info.PlayFabId}";
    }
}