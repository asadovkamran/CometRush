using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private StateMachine _stateMachine = new StateMachine();
    [SerializeField] private GameObject[] _uiStateObjects;
    [SerializeField] private GameObject _settingsMenuObject;
    [SerializeField] private GameOverSO _gameOverSO;
    [SerializeField] private GameStatsSO _gameStatsSO;

    private void Start()
    {
        _stateMachine.ChangeState(new MainMenuState(_uiStateObjects[0]), States.MainMenu);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void OnEnable()
    {
        _gameOverSO.PlayerDeadEvent.AddListener(HandlePlayerDeadEvent);
    }

    private void OnDisable()
    {
        _gameOverSO.PlayerDeadEvent.RemoveListener(HandlePlayerDeadEvent);
    }

    private void HandlePlayerDeadEvent()
    {
        _stateMachine.ChangeState(new GameOverState(_uiStateObjects[2]), States.GameOver);
    }

    public void OnPlayButton()
    {
        _gameStatsSO.ResetGameStats();
        HandlePlay();
    }

    public void OnQuitButton()
    {
        HandleQuit();
    }

    public void OnSettingsButton()
    {
        ToggleSettingsMenu();
    }

    private void HandlePlay()
    {
        _stateMachine.ChangeState(new GameplayState(_uiStateObjects[1]), States.Gameplay);
        SceneManager.LoadScene(1);
    }

    private void HandleQuit()
    {
        Application.Quit();
    }

    private void ToggleSettingsMenu()
    {
        if (_stateMachine.GetCurrentState() == States.MainMenu)
        {
            _uiStateObjects[0].SetActive(!_uiStateObjects[0].activeSelf);
        }

        if (_stateMachine.GetCurrentState() == States.Gameplay)
        {
            ToggleTimeScale();
            _gameStatsSO.UpdateGameplayUI();
        }

        _settingsMenuObject.SetActive(!_settingsMenuObject.activeSelf);
    }

    private void ToggleTimeScale()
    {
        Time.timeScale = 1 - Time.timeScale;
    }
}
