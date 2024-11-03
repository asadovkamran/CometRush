using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Slider _shieldsSlider;
    

    public static UIManager Instance;
    public GameConstants GAME_CONSTANTS;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScore(0);
        UpdateTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void OnEnable()
    {
        GameManager.OnShieldsUpdated += UpdateShieldsUI;
        GameManager.OnScoreUpdated += UpdateScore;
    }

    private void OnDisable()
    {
        GameManager.OnShieldsUpdated -= UpdateShieldsUI;
        GameManager.OnScoreUpdated -= UpdateScore;
    }

    public void UpdateScore(float score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateShieldsUI(float value)
    {
        _shieldsSlider.value = value / GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
    }

    private void UpdateTimer()
    {
        float elapsedTime = GameManager.elapsedTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);

        // Format and display the time in "MM:SS" format
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
