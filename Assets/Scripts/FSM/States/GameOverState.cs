using UnityEngine;

public class GameOverState : IState
{
    private readonly GameObject _targetUiObject;

    public GameOverState(GameObject targetUiObject)
    {
        _targetUiObject = targetUiObject;
    }

    public void Enter()
    {
        Time.timeScale = 0.0f;
        _targetUiObject.SetActive(true);
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        _targetUiObject.SetActive(false);
    }
}
