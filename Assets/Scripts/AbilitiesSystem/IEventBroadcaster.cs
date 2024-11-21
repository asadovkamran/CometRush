using UnityEngine;

public interface IEventBroadcaster
{
    public void Subscribe(System.Action<GameObject> listener);
    public void Unsubscribe(System.Action<GameObject> listener);
    public void Broadcast(GameObject source);
}