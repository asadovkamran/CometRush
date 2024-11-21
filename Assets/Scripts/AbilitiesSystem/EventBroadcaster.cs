using System;
using UnityEngine;

public class EventBroadcaster : IEventBroadcaster
{
    private event Action<GameObject> OnEventTriggered;
    public void Subscribe(Action<GameObject> listener)
    {
        OnEventTriggered += listener;
    }

    public void Unsubscribe(Action<GameObject> listener)
    {
        OnEventTriggered -= listener;
    }

    public void Broadcast(GameObject source)
    {
        OnEventTriggered?.Invoke(source);
    }
}