using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _explosionSoundAS;
    private AudioSource _cometImpactAS;
    private void Awake()
    {
        _explosionSoundAS = transform.Find("ExplosionSound").GetComponent<AudioSource>();
        _cometImpactAS = transform.Find("CometImpactSound").GetComponent<AudioSource>();
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
        _explosionSoundAS.Play();
    }

    private void HandleOnCometReachPlayer(float damage)
    {
        _cometImpactAS.Play();
    }
}
