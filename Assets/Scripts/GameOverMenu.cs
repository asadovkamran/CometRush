using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _hiscoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _playerStats;

    private void Start()
    {
        //float hiScore = PlayerPrefs.GetFloat("HiScore", 0);
        
        //_playerStats = GameObject.Find("PlayerStats");

        //float score = _playerStats.GetComponent<DontDestroyOnLoad>().GetScore();

        //if (score > hiScore)
        //{
        //    PlayerPrefs.SetFloat("HiScore", score);
        //    hiScore = PlayerPrefs.GetFloat("HiScore", 0);
        //}

        //_scoreText.text = "Score: " + score.ToString();
        //_hiscoreText.text = "Hi-score: " +hiScore.ToString();

        //float time = _playerStats.GetComponent<DontDestroyOnLoad>().GetTime();

        //int minutes = Mathf.FloorToInt(time / 60F);
        //int seconds = Mathf.FloorToInt(time % 60F);

        //_timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
