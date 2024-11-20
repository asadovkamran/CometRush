public class StateMachine
{
    private IState _currentState;
    private States _stateName;

    public void ChangeState(IState newState, States name) 
    {
        _currentState?.Exit();

        _currentState = newState;
        _stateName = name;
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState?.Execute();
    }

    public States GetCurrentState()
    {
        return _stateName;
    }
}
