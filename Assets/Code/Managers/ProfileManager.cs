using System;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public sealed class ProfileManager : MonoBehaviour
{
    [SerializeField] private ChangeUsernamePanelView _changeUsernamePanelView;
    [SerializeField] private GameObject _xpProgressBarObject;
    [SerializeField] private TMP_Text _usernameText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _nextLevelXpText;
    [SerializeField] private TMP_Text _currentXpText;
    [SerializeField] private Button _backButton;
    [SerializeField] private Image _xpProgressBar;

    private void Start()
    {
        _usernameText.text = "Getting user info...";
        _usernameText.color = Color.yellow;
        _xpProgressBarObject.gameObject.SetActive(false);
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
        }, Debug.LogError);
        
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), UpdateXp, Debug.LogError);
    }

    private void UpdateXp(GetUserDataResult result)
    {
        var data = result.Data;
        var xp = 0;
        var totalXp = 0;
        var level = 0;
        
        if (data.ContainsKey(Constants.SCORE_DATA_ID))
        {
            xp = Convert.ToInt32(data[Constants.SCORE_DATA_ID].Value);
        }
        
        if (data.ContainsKey(Constants.TOTAL_SCORE_DATA_ID))
        {
            totalXp = Convert.ToInt32(data[Constants.TOTAL_SCORE_DATA_ID].Value);
        }
        
        if (data.ContainsKey(Constants.LEVEL_DATA_ID))
        {
            level = Convert.ToInt32(data[Constants.LEVEL_DATA_ID].Value);
        }

        var totalXpForNextLevel = CountTotalXpForLevel(level + 1);
        var scoreForNextLevel = (int)(5 * Mathf.Pow(level, 2) + 50 * level + 100);

        _currentXpText.text = totalXp.ToString();
        _nextLevelXpText.text = totalXpForNextLevel.ToString();
        _levelText.text = level.ToString();
        _xpProgressBar.fillAmount = (float)xp / (float)scoreForNextLevel;
        _xpProgressBarObject.SetActive(true);
    }
    
    private int CountTotalXpForLevel(int levelToCount)
    {
        var totalXp = 0;

        for (var i = 0; i < levelToCount; ++i)
        {
            var level = Mathf.Clamp(i, 0, levelToCount + 1);

            totalXp += (int)(5 * Math.Pow(level, 2) + 50 * level + 100);
        }

        return totalXp;
    }

    private void SetUsernameText(string username)
    {
        _usernameText.text = $"{username}";
        _usernameText.color = Color.white;
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

    private async void GoToBootstrap()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        while (PhotonNetwork.NetworkClientState == ClientState.Leaving)
        {
            await Task.Delay(100);
        }
        
        SceneManager.LoadScene("Bootstrap");
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(GoToBootstrap);
        _changeUsernamePanelView.OnConfirmButtonClicked -= ChangeUsername;
    }
}