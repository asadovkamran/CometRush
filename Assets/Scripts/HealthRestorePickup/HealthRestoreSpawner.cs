using System;
using UnityEngine;

public class HealthRestoreSpawner : MonoBehaviour
{
    [SerializeField] private GameStatsSO _gameStatsSO;
    [SerializeField] private GameConstants GAME_CONSTANTS;
    [SerializeField] private GameObject _healthImg;

    private void Awake()
    {
        _healthImg.SetActive(false);
        
    }
    private void OnEnable()
    {
        HitDetection.OnCometHit += HandleHit;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= HandleHit;
    }

    private void HandleHit(GameObject obj)
    {
        if (CheckDropProbability(GAME_CONSTANTS.HEALTH_DROP_PROBABILITY) && _gameStatsSO.CurrentHealth
            < GAME_CONSTANTS.PLAYER_MAX_HEALTH)
        {
            Vector3 targetPosition = Camera.main.WorldToScreenPoint(obj.transform.position);
            _healthImg.transform.position = targetPosition;
            _healthImg.SetActive(true);
            
        }
    }

    private bool CheckDropProbability(float probability)
    {
        if (probability < 0 || probability > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0 and 100.");
        }

        float rnd = UnityEngine.Random.Range(0, 101);
        return rnd <= probability ? true : false;   
    }

    
}
