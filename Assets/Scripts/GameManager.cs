using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameConstants GAME_CONSTANTS;
    public int score = 0;
    [SerializeField] private float shieldsCapacity;
    [SerializeField] private float difficulty = 0;

    public event Action<float> OnShieldsUpdated;

    float time;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        shieldsCapacity = GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (shieldsCapacity <= 0) {
            HandleGameOver();
        }

        if (time > 1 && shieldsCapacity < GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY)
        {
            time = 0;
            UpdateShields(GAME_CONSTANTS.SHIELD_REGEN_RATE);
        }
        
    }

    public void UpdateShields(float amount)
    {
        shieldsCapacity += amount;
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
}
