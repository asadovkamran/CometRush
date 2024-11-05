using System.Collections;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public Transform Star;
    public GameConstants GAME_CONSTANTS;
    public ObjectPool Pool;
    
    private void Start()
    {
        StartCoroutine(SpawnStar());
    }

    IEnumerator SpawnStar()
    {
        while (true)
        {
            Pool.GetPooledObject();
            yield return new WaitForSeconds(Random.Range(GAME_CONSTANTS.STAR_SPAWN_INTERVAL_MIN_SECONDS, GAME_CONSTANTS.STAR_SPAWN_INTERVAL_MAX_SECONDS));
        }
    }
}
