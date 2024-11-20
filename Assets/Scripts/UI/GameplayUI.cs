using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeReference] private TextMeshProUGUI _timerText;
    [SerializeField] private GameStatsSO _gameStatsSO;

    private void Start()
    {
        UpdateScoreText(_gameStatsSO.Score);
    }

    private void Update()
    {
        UpdateTimer();   
    }

    private void OnEnable()
    {
        UpdateScoreText(_gameStatsSO.Score);
        _gameStatsSO.ScoreChangeEvent.AddListener(UpdateScoreText);
    }

    private void OnDisable()
    {
        _gameStatsSO.ScoreChangeEvent?.RemoveListener(UpdateScoreText);
    }

    private void UpdateScoreText(float value)
    {
        _scoreText.text = "Score: " + value;
    }

    private void UpdateTimer()
    {
        float elapsedTime = _gameStatsSO.ElapsedTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);

        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
