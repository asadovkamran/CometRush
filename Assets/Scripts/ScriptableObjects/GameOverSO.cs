using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameOverSO", menuName = "Scriptable Objects/GameOverSO")]
public class GameOverSO : ScriptableObject
{
    public UnityEvent PlayerDeadEvent;

    private void OnEnable()
    {
        PlayerDeadEvent = new UnityEvent();
    }

    public void OnPlayerDead()
    {
        PlayerDeadEvent?.Invoke();
    }
}
