using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Slider _shieldsSlider;
    [SerializeField] private Slider _healthSlider;

    public GameConstants GAME_CONSTANTS;
   
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

    public void UpdateShieldsUI(float shieldsAmount, float healthAmount)
    {
        _shieldsSlider.value = shieldsAmount / GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
        _healthSlider.value = healthAmount / GAME_CONSTANTS.PLAYER_MAX_HEALTH;
    }

    private void UpdateTimer()
    {
        float elapsedTime = GameManager.ElapsedTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
