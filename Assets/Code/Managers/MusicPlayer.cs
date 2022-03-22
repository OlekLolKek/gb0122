using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public sealed class MusicPlayer : MonoBehaviour, IMatchStateListener
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;
    
    [SerializeField] private AudioClip[] _mainMenuMusic;
    [SerializeField] private AudioClip[] _matchMusic;
    [SerializeField] private AudioClip _startMusic;
    [SerializeField] private AudioClip _startSound;
    [SerializeField] private AudioClip _endMusic;
    [SerializeField] private AudioClip _endSound;

    private MatchState _state;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);

        ChangeMatchState(MatchState.MainMenu);
    }

    private void Start()
    {
        var player = FindObjectOfType<MusicPlayer>();
        
        if (player != null && player != this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMatchState(MatchState matchState)
    {
        Debug.Log($"Changing state to {matchState}");
        
        _state = matchState;
        
        switch (matchState)
        {
            case MatchState.LoadingCountdown:
                PlayMusic(_startMusic, true);
                break;
            case MatchState.StartCountdown:
                break;
            case MatchState.MatchProcess:
                PlayMusic(GetRandomTrack(_matchMusic), true);
                PlaySfx(_startSound, false);
                break;
            case MatchState.EndCountdown:
                PlayMusic(_startMusic, true);
                break;
            case MatchState.EndScreen:
                PlayMusic(_endMusic, false);
                PlaySfx(_endSound, false);
                break;
            case MatchState.MainMenu:
                PlayMusic(GetRandomTrack(_mainMenuMusic), true);
                break;
        }
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        _musicAudioSource.clip = clip;
        _musicAudioSource.loop = loop;
        
        _musicAudioSource.Play();
    }
    
    private void PlaySfx(AudioClip clip, bool loop)
    {
        _sfxAudioSource.clip = clip;
        _sfxAudioSource.loop = loop;
        
        _sfxAudioSource.Play();
    }

    private AudioClip GetRandomTrack(IReadOnlyList<AudioClip> tracks)
    {
        return tracks[Random.Range(0, tracks.Count)];
    }

    public void SetMatchCountdown(float matchCountdown)
    {
        if (matchCountdown < Constants.MATCH_END_TIME)
        {
            if (_state != MatchState.EndCountdown && _state != MatchState.EndScreen)
            {
                ChangeMatchState(MatchState.EndCountdown);
            }
        }
    }
}