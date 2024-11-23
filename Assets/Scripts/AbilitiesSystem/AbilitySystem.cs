using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    private EventBroadcaster _asteroidDestroyedEventBroadcaster = new();
    private ProcManager _procManager = new();

    private void Start()
    {
        _procManager.AddAbility(new FreezeAsteroids(), 0.1f);

        _asteroidDestroyedEventBroadcaster.Subscribe(_procManager.TryProc);
    }

    public void OnAsteroidDestroyed(GameObject asteroid)
    {
        _asteroidDestroyedEventBroadcaster.Broadcast(asteroid);
    }
}
