using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float shieldsCapacity;
    [SerializeField] private float difficulty = 0;

    public static GameManager Instance;
    public GameConstants GAME_CONSTANTS;

    public static event Action<float> OnShieldsUpdated;
    public static event Action<float> OnScoreUpdated;

    public static int score = 0;
    public static float elapsedTime;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        shieldsCapacity = GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
        InvokeRepeating("RegenerateShields", 2f, GAME_CONSTANTS.SHIELD_REGEN_RATE);
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += IncrementScore;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit += IncrementScore;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (shieldsCapacity <= 0)
        {
            HandleGameOver();
        }
    }

    public void UpdateShields(float amount)
    {
        shieldsCapacity += amount;
        OnShieldsUpdated?.Invoke(shieldsCapacity);
    }

    private void RegenerateShields()
    {
        shieldsCapacity = Mathf.Clamp(shieldsCapacity + 1, 0f, 100f);
        OnShieldsUpdated?.Invoke(shieldsCapacity);
    }

    public float GetShields() { return shieldsCapacity; }

    void HandleGameOver()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(2);
    }

    public float getDifficulty()
    {
        difficulty += Time.deltaTime * GAME_CONSTANTS.DIFFICULTY_COEFFICIENT;
        return difficulty;
    }

    private void IncrementScore(GameObject obj)
    {
        score++;
        OnScoreUpdated?.Invoke(score);
    }
}
