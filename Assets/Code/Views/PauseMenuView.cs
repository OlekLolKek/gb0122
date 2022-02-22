using UnityEngine;
using UnityEngine.UI;


public sealed class PauseMenuView : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private ChangeUsernamePanelView _changeUsernamePanel;
    
    [Header("Buttons")]
    [SerializeField] private Button _closeMenuButton;
    [SerializeField] private Button _changeUsernameButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _closeMenuButton.onClick.AddListener(CloseMenuButtonClicked);
        _changeUsernameButton.onClick.AddListener(ChangeUsernameButtonClicked);
        _quitButton.onClick.AddListener(QuitButtonClicked);

        _changeUsernamePanel.OnBackButtonClicked += CloseChangeUsernamePanel;
        _changeUsernamePanel.OnConfirmButtonClicked += ConfirmChangeUsernameButtonClicked;
    }

    private void OnDestroy()
    {
        _closeMenuButton.onClick.RemoveListener(CloseMenuButtonClicked);
        _changeUsernameButton.onClick.RemoveListener(ChangeUsernameButtonClicked);
        _quitButton.onClick.RemoveListener(QuitButtonClicked);
    }

    public void Activate()
    {
        _changeUsernamePanel.gameObject.SetActive(false);
        _changeUsernameButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
        
        gameObject.SetActive(true);
    }

    private void ChangeUsernameButtonClicked()
    {
        _changeUsernamePanel.gameObject.SetActive(true);
        _changeUsernameButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
    }

    private void CloseChangeUsernamePanel()
    {
        _changeUsernamePanel.gameObject.SetActive(false);
        _changeUsernameButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    private void ConfirmChangeUsernameButtonClicked(string _)
    {
        _changeUsernamePanel.gameObject.SetActive(false);
        _changeUsernameButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    private void CloseMenuButtonClicked()
    {
        _changeUsernamePanel.gameObject.SetActive(false);
        _changeUsernameButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        
        gameObject.SetActive(false);
    }

    private void QuitButtonClicked()
    {
        Application.Quit();
    }
}