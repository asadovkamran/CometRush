using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _explosionSoundClip;

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
    
    private void Awake()
    {
        _cometImpactAS = transform.Find("CometImpactSound").GetComponent<AudioSource>();

        for (int i = 0; i < 3; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = _explosionSoundClip;
            _bufferAudioSources.Add(source);
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
}
