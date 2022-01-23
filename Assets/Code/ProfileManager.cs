using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;


public sealed class ProfileManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _idText;
    
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), success =>
        {
            _idText.text = $"Welcome back, Player {success.AccountInfo.PlayFabId}";
        }, error =>
        {
            _idText.text = $"Something went wrong: {error}";
        });
    }
}