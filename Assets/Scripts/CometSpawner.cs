using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public Transform comet;
    public GameConstants GAME_CONSTANTS;

    private List<Transform> activeComets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnComet());
    }


    IEnumerator SpawnComet()
    {
        while (true)
        {
            activeComets = activeComets.Where(comet => !comet.IsDestroyed()).ToList();

            if (activeComets.Count < GAME_CONSTANTS.MAX_SIMULTANEOUS_COMETS + Mathf.Floor(GameManager.Instance.getDifficulty()))
            {
                Transform newComet = Instantiate(comet);
                activeComets.Add(newComet);
            }
            yield return new WaitForSeconds(
                Random.Range(GAME_CONSTANTS.COMET_SPAWN_INTERVAL_MIN_SECONDS, GAME_CONSTANTS.COMET_SPAWN_INTERVAL_MAX_SECONDS)
            );
        }
    }
}
