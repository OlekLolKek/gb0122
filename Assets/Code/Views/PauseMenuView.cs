using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public sealed class PauseMenuView : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private ChangeUsernamePanelView _changeUsernamePanel;

    [Header("Controls")]
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Button _closeMenuButton;
    [SerializeField] private Button _changeUsernameButton;
    [SerializeField] private Button _quitButton;

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _musicVolumeKey;
    [SerializeField] private string _soundVolumeKey;

    private void Start()
    {
        _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        _soundVolumeSlider.onValueChanged.AddListener(SoundVolumeChanged);
        _closeMenuButton.onClick.AddListener(CloseMenuButtonClicked);
        _changeUsernameButton.onClick.AddListener(ChangeUsernameButtonClicked);
        _quitButton.onClick.AddListener(QuitButtonClicked);

        _changeUsernamePanel.OnBackButtonClicked += CloseChangeUsernamePanel;
        _changeUsernamePanel.OnConfirmButtonClicked += ConfirmChangeUsernameButtonClicked;

        SetSliderValues();
    }

    private void OnDestroy()
    {
        _musicVolumeSlider.onValueChanged.RemoveListener(MusicVolumeChanged);
        _soundVolumeSlider.onValueChanged.RemoveListener(SoundVolumeChanged);
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

    private void SetSliderValues()
    {
        _audioMixer.GetFloat(_musicVolumeKey, out var musicVolume);
        _musicVolumeSlider.value = GetSliderValue(musicVolume);
        
        _audioMixer.GetFloat(_soundVolumeKey, out var soundVolume);
        _soundVolumeSlider.value = GetSliderValue(soundVolume);
    }

    private void MusicVolumeChanged(float sliderValue)
    {
        _audioMixer.SetFloat(_musicVolumeKey, GetDecibels(sliderValue));
    }

    private void SoundVolumeChanged(float sliderValue)
    {
        _audioMixer.SetFloat(_soundVolumeKey, GetDecibels(sliderValue));
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

    private float GetDecibels(float sliderValue)
    {
        return Mathf.Log10(sliderValue) * 20;
    }

    private float GetSliderValue(float decibels)
    {
        var log10 = decibels / 20;
        return Mathf.Pow(10, log10);
    }

    private void QuitButtonClicked()
    {
        Application.Quit();
    }
}