using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _shieldsSlider;
    public GameConstants GAME_CONSTANTS;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScore(0);
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
}
