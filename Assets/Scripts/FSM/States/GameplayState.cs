using UnityEngine;

public class GameplayState : IState
{
    private readonly GameObject _targetUiObject;

    public GameplayState(GameObject targetUiObject)
    {
        _targetUiObject = targetUiObject;
    }

    public void Enter()
    {
        Time.timeScale = 1.0f;
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
