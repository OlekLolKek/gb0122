using System.Threading.Tasks;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public sealed class ProfileManager : MonoBehaviour
{
    [SerializeField] private ChangeUsernamePanelView _changeUsernamePanelView;
    [SerializeField] private TMP_Text _usernameText;
    [SerializeField] private TMP_Text _idText;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        _usernameText.text = "Getting user info...";
        _usernameText.color = Color.yellow;
        _idText.text = string.Empty;
        UpdateUserInfo();

        _backButton.onClick.AddListener(GoToBootstrap);
        _changeUsernamePanelView.OnConfirmButtonClicked += ChangeUsername;
    }

    private async void UpdateUserInfo()
    {
        // PlayFab information needs some time to update, so the delay is necessary to get the most recent info
        await Task.Delay(250);
        
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), result =>
        {
            SetPhotonNickname(result.PlayerProfile.DisplayName);
            SetUsernameText(result.PlayerProfile.DisplayName);
            SetIdText(result.PlayerProfile.PlayerId);
        }, Debug.LogError);
    }

    private void SetUsernameText(string username)
    {
        _usernameText.text = $"{username}";
        _usernameText.color = Color.white;
    }
    
    private void SetIdText(string id)
    {
        _idText.text = $"{id}";
        _idText.color = Color.white;
    }

    private void SetPhotonNickname(string nickName)
    {
        PhotonNetwork.NickName = nickName;
    }

    private void ChangeUsername(string newUsername)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = newUsername },
            result =>
            {
                SetUsernameText(result.DisplayName);
                SetPhotonNickname(result.DisplayName);
            }, Debug.LogError);
    }

    private void GoToBootstrap()
    {
        SceneManager.LoadScene("Bootstrap");
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(GoToBootstrap);
        _changeUsernamePanelView.OnConfirmButtonClicked -= ChangeUsername;
    }
}