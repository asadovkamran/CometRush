using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameStatsSO", menuName = "Scriptable Objects/GameStatsSO")]
public class GameStatsSO : ScriptableObject
{
    [SerializeField] private GameConstants GAME_CONSTANTS;

    public float Score;
    public float CurrentHealth;
    public float CurrentShields;
    public float ElapsedTime;

    private float _maxHealth;
    private float _maxShields;

    public UnityEvent<float> ScoreChangeEvent;
    public UnityEvent<float> CurrentHealthChangeEvent;
    public UnityEvent<float> CurrentShieldsChangeEvent;

    private void OnEnable()
    {
        ScoreChangeEvent ??= new UnityEvent<float>();

        CurrentHealthChangeEvent ??= new UnityEvent<float>();

        CurrentShieldsChangeEvent ??= new UnityEvent<float>();

        Score = 0;
        ElapsedTime = 0;
        _maxHealth = GAME_CONSTANTS.PLAYER_MAX_HEALTH;
        _maxShields = GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
        CurrentHealth = _maxHealth;
        CurrentShields = _maxShields;
    }

    public void IncrementScore()
    {
        Score++;
        ScoreChangeEvent?.Invoke(Score);
    }

    public void UpdateHealth() {
        
        CurrentHealthChangeEvent?.Invoke(CurrentHealth);
    }

    public void UpdateShields() { 
        CurrentShieldsChangeEvent?.Invoke(CurrentShields);
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetMaxShields()
    {
        return _maxShields;
    }
}
