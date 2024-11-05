using UnityEngine;

public class CometImpactSound : MonoBehaviour
{
    public static CometImpactSound Instance;

    private AudioSource _audio;

    private void Awake()
    {
        Instance = this;
        _audio = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        _audio.Play();
    }
}
