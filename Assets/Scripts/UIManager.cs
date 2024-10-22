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
        GameManager.Instance.OnShieldsUpdated += UpdateShieldsUI;
        UpdateScore();
    }

    public void UpdateScore()
    {
        _scoreText.text = "Score: " + GameManager.Instance.score;
    }

    public void TestingEvent()
    {
        Debug.Log("Comet crashed event");
    }

    public void UpdateShieldsUI(float value)
    {
        Debug.Log("update shields ui");
        Debug.Log(value / GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY);
        _shieldsSlider.value = value / GAME_CONSTANTS.PLAYER_SHIELDS_CAPACITY;
    }
}
