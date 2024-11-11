using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _shieldsCapacity;
    [SerializeField] private float _health;
    [SerializeField] private float _difficulty = 0;

    public static GameManager Instance;
    public GameConstants GAME_CONSTANTS;

    public static event Action<float, float> OnShieldsUpdated;
    public static event Action<float> OnScoreUpdated;
    public static event Action<int, float> OnGameOver;

    public static int Score = 0;
    public static float ElapsedTime;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _shieldsCapacity = GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
        _health = GAME_CONSTANTS.PLAYER_MAX_HEALTH;
        InvokeRepeating("RegenerateShields", 2f, GAME_CONSTANTS.SHIELD_REGEN_RATE);
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += IncrementScore;
        Comet.OnCometReachPlayer += HandleOnCometReachPlayer;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= IncrementScore;
        Comet.OnCometReachPlayer -= HandleOnCometReachPlayer;
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        } 
    }

    private void HandleOnCometReachPlayer(float damageAmount)
    {
        if (_health <= 0)
        {
            HandleGameOver();
        }

        if (_shieldsCapacity < damageAmount)
        {
            _shieldsCapacity = 0;
            _health -= damageAmount - _shieldsCapacity;
        } else
        {
            _shieldsCapacity -= damageAmount;
        }
     
        OnShieldsUpdated?.Invoke(_shieldsCapacity, _health);
    }

    public void UpdateShields(float amount)
    {
        _shieldsCapacity += amount;
        
    }

    private void RegenerateShields()
    {
        _shieldsCapacity = Mathf.Clamp(_shieldsCapacity + 1, 0f, 100f);
        OnShieldsUpdated?.Invoke(_shieldsCapacity, _health);
    }

    public float GetShields() { return _shieldsCapacity; }

    private void HandleGameOver()
    {
        Time.timeScale = 0;
        OnGameOver?.Invoke(Score, ElapsedTime);
        SceneManager.LoadScene(2);
    }

    public float getDifficulty()
    {
        _difficulty += Time.deltaTime * GAME_CONSTANTS.DIFFICULTY_COEFFICIENT;
        return _difficulty;
    }

    private void IncrementScore(GameObject obj)
    {
        Score++;
        OnScoreUpdated?.Invoke(Score);
    }
}
