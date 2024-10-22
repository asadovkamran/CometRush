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

    public event Action<float> OnShieldsUpdated;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        shieldsCapacity = GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (shieldsCapacity <= 0) {
            HandleGameOver();
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
        Debug.Log("Game over");
        Time.timeScale = 0;
    }
}
