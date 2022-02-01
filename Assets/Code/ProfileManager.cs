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
    [SerializeField] private Button _changeUsernameButton;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        _usernameText.text = "Getting user info...";
        _usernameText.color = Color.yellow;
        _idText.text = string.Empty;
        UpdateUserInfo();

        _backButton.onClick.AddListener(GoToBootstrap);
        _changeUsernameButton.onClick.AddListener(OpenChangeUsernamePanel);
        _changeUsernamePanelView.OnConfirmButtonClicked += ChangeUsername;
    }

    private void UpdateUserInfo()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), result =>
        {
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

    private void OpenChangeUsernamePanel()
    {
        _changeUsernamePanelView.gameObject.SetActive(true);
    }

    private void ChangeUsername(string newUsername)
    {
        _changeUsernamePanelView.gameObject.SetActive(false);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = newUsername },
            result =>
            {
                SetUsernameText(result.DisplayName);
            }, Debug.LogError);
    }

    private void GoToBootstrap()
    {
        SceneManager.LoadScene("Bootstrap");
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(GoToBootstrap);
        _changeUsernameButton.onClick.RemoveListener(OpenChangeUsernamePanel);
        _changeUsernamePanelView.OnConfirmButtonClicked -= ChangeUsername;
    }
}