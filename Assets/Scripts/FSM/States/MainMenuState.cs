using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : IState
{
    private UIManager _owner;
    private GameObject _targetUiObject;

    public MainMenuState(UIManager owner, GameObject targetUiObject)
    {
        _owner = owner;
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
