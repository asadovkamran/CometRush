using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _hiscoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameStatsSO _gameStatsSO;

    private void Start()
    {
        float hiScore = PlayerPrefs.GetFloat("HiScore", 0);

        float score = _gameStatsSO.Score;

        if (score > hiScore)
        {
            PlayerPrefs.SetFloat("HiScore", score);
            hiScore = PlayerPrefs.GetFloat("HiScore", 0);
        }

        _scoreText.text = "Score: " + score;
        _hiscoreText.text = "Hi-score: " + hiScore;

        float time = _gameStatsSO.ElapsedTime;

        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);

        _timerText.text = "Time: " + $"{minutes:00}:{seconds:00}";
    }
}
