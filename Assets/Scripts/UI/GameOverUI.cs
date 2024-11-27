using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _hiscoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameStatsSO _gameStatsSO;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        int hiScore = PlayerPrefs.GetInt("HiScore", 0);

        int score = _gameStatsSO.Score;

        if (score > hiScore)
        {
            PlayerPrefs.SetInt("HiScore", score);
            hiScore = PlayerPrefs.GetInt("HiScore", 0);
        }

        _gameStatsSO.HiScore = hiScore;

        _scoreText.text = "Score: " + score;
        _hiscoreText.text = "Hi-score: " + hiScore;

        float time = _gameStatsSO.ElapsedTime;

        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);

        _timerText.text = "Time: " + $"{minutes:00}:{seconds:00}";
    }
}
