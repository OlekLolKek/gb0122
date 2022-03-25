using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public sealed class PauseMenuView : MonoBehaviour
{
    [Header("Panels")] [SerializeField] private ChangeUsernamePanelView _changeUsernamePanel;

    [Header("Controls")] [SerializeField] private TMP_Text _musicVolumeText;
    [SerializeField] private TMP_Text _soundVolumeText;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Button _closeMenuButton;
    [SerializeField] private Button _changeUsernameButton;
    [SerializeField] private Button _quitButton;

    [Header("Audio Mixer Groups")] [SerializeField]
    private AudioMixer _audioMixer;

    [SerializeField] private string _musicVolumeKey;
    [SerializeField] private string _soundVolumeKey;

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(Constants.MUSIC_VOLUME_PREFS_KEY) &&
            PlayerPrefs.HasKey(Constants.SOUND_VOLUME_PREFS_KEY))
        {
            LoadSettings();
        }
        else
        {
            SetSliderValues();
        }
    }

    private void Start()
    {
        _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        _soundVolumeSlider.onValueChanged.AddListener(SoundVolumeChanged);
        _closeMenuButton.onClick.AddListener(CloseMenuButtonClicked);
        _changeUsernameButton.onClick.AddListener(ChangeUsernameButtonClicked);
        _quitButton.onClick.AddListener(QuitButtonClicked);

        _changeUsernamePanel.OnBackButtonClicked += CloseChangeUsernamePanel;
        _changeUsernamePanel.OnConfirmButtonClicked += ConfirmChangeUsernameButtonClicked;
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
        _changeUsernamePanel.Deactivate();

        _changeUsernameButton.gameObject.SetActive(true);
        _musicVolumeSlider.gameObject.SetActive(true);
        _soundVolumeSlider.gameObject.SetActive(true);
        _musicVolumeText.gameObject.SetActive(true);
        _soundVolumeText.gameObject.SetActive(true);
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
        _changeUsernamePanel.Activate();

        _changeUsernameButton.gameObject.SetActive(false);
        _musicVolumeSlider.gameObject.SetActive(false);
        _soundVolumeSlider.gameObject.SetActive(false);
        _musicVolumeText.gameObject.SetActive(false);
        _soundVolumeText.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
    }

    private void CloseChangeUsernamePanel()
    {
        _changeUsernamePanel.Deactivate();

        _changeUsernameButton.gameObject.SetActive(true);
        _musicVolumeSlider.gameObject.SetActive(true);
        _soundVolumeSlider.gameObject.SetActive(true);
        _musicVolumeText.gameObject.SetActive(true);
        _soundVolumeText.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    private void ConfirmChangeUsernameButtonClicked(string _)
    {
        _changeUsernamePanel.Deactivate();

        _changeUsernameButton.gameObject.SetActive(true);
        _musicVolumeSlider.gameObject.SetActive(true);
        _soundVolumeSlider.gameObject.SetActive(true);
        _musicVolumeText.gameObject.SetActive(true);
        _soundVolumeText.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    private void CloseMenuButtonClicked()
    {
        _changeUsernamePanel.Deactivate();

        _changeUsernameButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);

        gameObject.SetActive(false);

        SaveSettings();
    }

    private void SaveSettings()
    {
        _audioMixer.GetFloat(_soundVolumeKey, out var sound);
        PlayerPrefs.SetFloat(Constants.SOUND_VOLUME_PREFS_KEY, sound);

        _audioMixer.GetFloat(_musicVolumeKey, out var music);
        PlayerPrefs.SetFloat(Constants.MUSIC_VOLUME_PREFS_KEY, music);
    }

    private void LoadSettings()
    {
        var soundVolume = PlayerPrefs.GetFloat(Constants.SOUND_VOLUME_PREFS_KEY);
        _audioMixer.SetFloat(_soundVolumeKey, soundVolume);
        _soundVolumeSlider.value = GetSliderValue(soundVolume);
        
        var musicVolume = PlayerPrefs.GetFloat(Constants.MUSIC_VOLUME_PREFS_KEY);
        _audioMixer.SetFloat(_musicVolumeKey, musicVolume);
        _musicVolumeSlider.value = GetSliderValue(musicVolume);
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