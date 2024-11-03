using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource explosionSound;

    private void Awake()
    {
        explosionSound = GetComponent<AudioSource>();
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
        explosionSound.Play();
    }
}
