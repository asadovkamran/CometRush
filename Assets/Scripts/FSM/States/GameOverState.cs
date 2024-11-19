using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IState
{
    private UIManager _owner;
    private GameObject _targetUiObject;

    public GameOverState(UIManager owner, GameObject targetUiObject)
    {
        _owner = owner;
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
