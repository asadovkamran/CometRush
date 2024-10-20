using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        _scoreText.text = "Score: " + GameManager.Instance.score;
    }
}
