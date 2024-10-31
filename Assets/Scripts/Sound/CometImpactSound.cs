using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometImpactSound : MonoBehaviour
{
    public static CometImpactSound Instance;
    private AudioSource audio;

    private void Awake()
    {
        Instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audio.Play();
    }
}
