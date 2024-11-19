using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private StateMachine _stateMachine = new StateMachine();
    [SerializeField] private GameObject[] _uiStateObjects;
    [SerializeField] private GameObject _settingsMenuObject;
    [SerializeField] private GameOverSO _gameOverSO;

    private void Start()
    {
        _stateMachine.ChangeState(new MainMenuState(this, _uiStateObjects[0]), States.MainMenu);
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
        _stateMachine.ChangeState(new GameOverState(this, _uiStateObjects[2]), States.GameOver);
    }

    public void OnPlayButton()
    {
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
        _stateMachine.ChangeState(new GameplayState(this, _uiStateObjects[1]), States.Gameplay);
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
        _settingsMenuObject.SetActive(!_settingsMenuObject.activeSelf);
    }
}
