using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameStatsSO", menuName = "Scriptable Objects/GameStatsSO")]
public class GameStatsSO : ScriptableObject
{
    [SerializeField] private GameConstants GAME_CONSTANTS;

    public int Score;
    public int HiScore;
    public float CurrentHealth;
    public float CurrentShields;
    public float ElapsedTime;

    private float _maxHealth;
    private float _maxShields;

    public UnityEvent<float> ScoreChangeEvent;
    public UnityEvent<float> CurrentHealthChangeEvent;
    public UnityEvent<float> CurrentShieldsChangeEvent;
    public UnityEvent GamePausedEvent;

    private void OnEnable()
    {
        ScoreChangeEvent ??= new UnityEvent<float>();

        CurrentHealthChangeEvent ??= new UnityEvent<float>();

        CurrentShieldsChangeEvent ??= new UnityEvent<float>();

        GamePausedEvent ??= new UnityEvent();

        ResetGameStats();
    }

    public void ResetGameStats()
    {
        Score = 0;
        ElapsedTime = 0;
        _maxHealth = GAME_CONSTANTS.PLAYER_MAX_HEALTH;
        _maxShields = GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
        CurrentHealth = _maxHealth;
        CurrentShields = _maxShields;
        UpdateHealth();
        UpdateShields();
        UpdateScore();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScore();
    }

    public void UpdateScore()
    {
        ScoreChangeEvent?.Invoke(Score);
    }

    public void UpdateHealth() {
        
        CurrentHealthChangeEvent?.Invoke(CurrentHealth);
    }

    public void UpdateShields() { 
        CurrentShieldsChangeEvent?.Invoke(CurrentShields);
    }

    public void UpdateGameplayUI()
    {
        GamePausedEvent?.Invoke();
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
