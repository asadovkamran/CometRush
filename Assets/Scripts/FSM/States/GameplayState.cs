using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : IState
{
    private UIManager _owner;
    private GameObject _targetUiObject;

    public GameplayState(UIManager owner, GameObject targetUiObject)
    {
        _owner = owner;
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
