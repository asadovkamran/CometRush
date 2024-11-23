using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CometRush.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    [SerializeField] private CometConfigSO _cometConfigSO;
    [SerializeField] private Transform _defaultCometTransform;
    [SerializeField] private Transform _iceCometTransform;
    private float _iceCometSpawnProbability;    

    public static CometSpawner Instance { get; private set; }
    
    public GameConstants GAME_CONSTANTS;

    public List<Transform> ActiveComets = new List<Transform>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _iceCometSpawnProbability = _cometConfigSO.IceCometSpawnProbabilty;
    }

    private void OnEnable()
    {
        _cometConfigSO.CometHitEvent.AddListener(HandleCometHit);
    }

    private void OnDisable()
    {
        _cometConfigSO.CometHitEvent.RemoveListener(HandleCometHit);
    }

    private void Start()
    {
        StartCoroutine(SpawnComet());
    }


    IEnumerator SpawnComet()
    {
        while (true)
        {
            ActiveComets = ActiveComets.Where(comet => !comet.IsDestroyed()).ToList();

            if (ActiveComets.Count < GAME_CONSTANTS.MAX_SIMULTANEOUS_COMETS + Mathf.Floor(GameManager.Instance.getDifficulty()))
            {
                Transform newComet = SpawnCometWithProbability();
                ActiveComets.Add(newComet);
            }
            yield return new WaitForSeconds(
                Random.Range(GAME_CONSTANTS.COMET_SPAWN_INTERVAL_MIN_SECONDS, GAME_CONSTANTS.COMET_SPAWN_INTERVAL_MAX_SECONDS)
            );
        }
    }

    private Transform SpawnCometWithProbability()
    {
        if (Random.value < _iceCometSpawnProbability)
        {
            return Instantiate(_iceCometTransform);
        }

        return Instantiate(_defaultCometTransform);
    }

    private void HandleCometHit(CometType type)
    {
        switch (type)
        {
            case CometType.Ice:
                FreezeAllActiveComets();
                break;
        }
    }

    private void FreezeAllActiveComets()
    {
        var activeComets = ActiveComets.Where(comet => !comet.IsDestroyed()).ToList();

        foreach (var comet in activeComets)
        {
            var obj = comet.GetComponent<Comet>();
            obj.Freeze();
            obj.SetMaterial(_cometConfigSO.IceCometMaterial);
        }
    }
}
