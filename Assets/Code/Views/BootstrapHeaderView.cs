using UnityEngine;
using UnityEngine.UI;


public sealed class BootstrapHeaderView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;

    [Header("Panels")]
    [SerializeField] private BootstrapPauseMenuView _pauseMenu;

    private void Start()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _pauseMenu.LoadData();
    }

    private void OnDestroy()
    {
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
    }

    private void OnSettingsButtonClicked()
    {
        _pauseMenu.Activate();
    }
}