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
            _idText.text = $"{success.AccountInfo.Username}\nID: {success.AccountInfo.PlayFabId}";
            _idText.color = Color.white;
        }, error =>
        {
            _idText.text = $"Something went wrong: {error}";
            _idText.color = Color.red;
        });
        
        _backButton.onClick.AddListener(GoToBootstrap);
    }

    private void GoToBootstrap()
    {
        SceneManager.LoadScene("Bootstrap");
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(GoToBootstrap);
    }
}