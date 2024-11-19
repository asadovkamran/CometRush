using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine
{
    private IState _currenState;
    private States _stateName;

    public void ChangeState(IState newState, States name) 
    {
        if (_currenState != null)
        {
            _currenState.Exit();
        }

        _currenState = newState;
        _stateName = name;
        _currenState.Enter();
    }

    public void Update()
    {
        _currenState?.Execute();
    }

    public States GetCurrentState()
    {
        return _stateName;
    }
}
