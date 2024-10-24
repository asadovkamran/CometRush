using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public Transform star;

    public GameConstants GAME_CONSTANTS;
    public ObjectPool pool;
    
    void Start()
    {
        StartCoroutine(SpawnStar());
    }

    IEnumerator SpawnStar()
    {
        while (true)
        {
            pool.GetPooledObject();
            yield return new WaitForSeconds(Random.Range(GAME_CONSTANTS.STAR_SPAWN_INTERVAL_MIN_SECONDS, GAME_CONSTANTS.STAR_SPAWN_INTERVAL_MAX_SECONDS));
        }
    }
}
