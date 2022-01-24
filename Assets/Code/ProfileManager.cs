using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public sealed class ProfileManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _idText;
    [SerializeField] private Button _backButton;
    
    private void Start()
    {
        _idText.text = "Getting user info...";
        _idText.color = Color.yellow;
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), success =>
        {
            _idText.text = $"Welcome back, Player {success.AccountInfo.PlayFabId}\n{GetPlayerInfo(success.AccountInfo)}";
            _idText.color = Color.white;
        }, error =>
        {
            _idText.text = $"Something went wrong: {error}";
        });
        
        _backButton.onClick.AddListener(GoToBootstrap);
    }

    private void GoToBootstrap()
    {
        SceneManager.LoadScene("Bootstrap");
    }

    private string GetPlayerInfo(UserAccountInfo info)
    {
        return $"Account created on {info.Created}\nCustom id: {info.CustomIdInfo.CustomId}\nUsername: {info.Username}";
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(GoToBootstrap);
    }
}