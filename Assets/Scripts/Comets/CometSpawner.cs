using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public Transform Comet;
    public GameConstants GAME_CONSTANTS;

    private List<Transform> _activeComets = new List<Transform>();

    private void Start()
    {
        StartCoroutine(SpawnComet());
    }


    IEnumerator SpawnComet()
    {
        while (true)
        {
            _activeComets = _activeComets.Where(comet => !comet.IsDestroyed()).ToList();

            if (_activeComets.Count < GAME_CONSTANTS.MAX_SIMULTANEOUS_COMETS + Mathf.Floor(GameManager.Instance.getDifficulty()))
            {
                Transform newComet = Instantiate(Comet);
                _activeComets.Add(newComet);
            }
            yield return new WaitForSeconds(
                Random.Range(GAME_CONSTANTS.COMET_SPAWN_INTERVAL_MIN_SECONDS, GAME_CONSTANTS.COMET_SPAWN_INTERVAL_MAX_SECONDS)
            );
        }
    }
}
