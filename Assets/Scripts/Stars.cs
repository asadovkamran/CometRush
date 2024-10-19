using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public Transform star;

    public GameConstants GAME_CONSTANTS;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnStar());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnStar()
    {
        while (true)
        {
            Instantiate(star);
            yield return new WaitForSeconds(Random.Range(GAME_CONSTANTS.STAR_SPAWN_INTERVAL_MIN_SECONDS, GAME_CONSTANTS.STAR_SPAWN_INTERVAL_MAX_SECONDS));
        }
    }
}
