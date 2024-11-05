using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _explosionSound;

    private void Awake()
    {
        _explosionSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += HandleSoundPlay;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= HandleSoundPlay;
    }

    private void HandleSoundPlay(GameObject obj)
    {
        _explosionSound.Play();
    }
}
