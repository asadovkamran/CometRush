using UnityEngine;

public class MainMenuState : IState
{
    private readonly GameObject _targetUiObject;

    public MainMenuState(GameObject targetUiObject)
    {
        _targetUiObject = targetUiObject;
    }

    public void Enter()
    {
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
