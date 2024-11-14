using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _explosionSoundClip;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioSource _backgroundMusicAudioSource;
    [SerializeField] private AudioClip[] _backgroundTracks;

    [SerializeField] private Slider _masterVolumeSlider;

    // when player hits comets in a very short time range
    // explosion sounds get clipped because they are played in one audio source
    // to fix this we add extra audio sources and every time we play explosion sound
    // we do it on the audio source that is free i.e. doesnt play anything
    private List<AudioSource> _bufferAudioSources = new List<AudioSource>(); // list to keep extra audio sources

    // index to iterate through the list of audio sources
    // not sure why but iterating for foreach and checking isPlaying didn't work 
    // so i iterate manually (see HandleCometHitMethod)
    private int _currentSourceIndex; 
    private AudioSource _cometImpactAS;

    private int _currentTrack = 0;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _cometImpactAS = transform.Find("CometImpactSound").GetComponent<AudioSource>();

        for (int i = 0; i < 3; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = _audioMixerGroup;
            source.clip = _explosionSoundClip;
            _bufferAudioSources.Add(source);
        }
    }

    private void Start()
    {
        PlayRandomBgTrack();
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            SetMasterVolume();
        }
    }

    private void Update()
    {
        if (!_backgroundMusicAudioSource.isPlaying)
        {
            PlayNextTrack(_currentTrack);
        }
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += HandleOnCometHit;
        Comet.OnCometReachPlayer += HandleOnCometReachPlayer;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= HandleOnCometHit;
        Comet.OnCometReachPlayer -= HandleOnCometReachPlayer;
    }

    private void HandleOnCometHit(GameObject obj)
    {
        if (_bufferAudioSources.Count == 0) return;

        AudioSource source = _bufferAudioSources[_currentSourceIndex];

        if (source != null && !source.isPlaying)
        {
            source.Play();

            _currentSourceIndex = (_currentSourceIndex + 1) % _bufferAudioSources.Count;
        }
    }

    private void HandleOnCometReachPlayer(float damage)
    {
        _cometImpactAS.Play();
    }

    public void SetMasterVolume()
    {
        if (_masterVolumeSlider != null)
        {
            float masterVolumeSliderValue = _masterVolumeSlider.value;
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSliderValue) * 20);
           
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSliderValue);
        }
    }

    private void PlayRandomBgTrack()
    {
        _backgroundTracks = ShuffleTracks(_backgroundTracks);
        _currentTrack = Random.Range(0, _backgroundTracks.Length);
        _backgroundMusicAudioSource.clip = _backgroundTracks[_currentTrack];
        _backgroundMusicAudioSource.Play();
    }

    private void PlayNextTrack(int currenTrack)
    {
        _currentTrack++;
        if (currenTrack == _backgroundTracks.Length)
        {
            _currentTrack = 0;
        }

        _backgroundMusicAudioSource.clip = _backgroundTracks[_currentTrack];
        _backgroundMusicAudioSource.Play();
    }

    private AudioClip[] ShuffleTracks(AudioClip[] tracks)
    {
        for (int t = 0; t < tracks.Length; t++)
        {
            AudioClip tmp = tracks[t];
            int r = Random.Range(t, tracks.Length);
            tracks[t] = tracks[r];
            tracks[r] = tmp;
        }

        return tracks;
    }
}
