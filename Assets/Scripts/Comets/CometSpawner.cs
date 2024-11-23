using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CometRush.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    [Header("Game Constants")]
    public GameConstants GAME_CONSTANTS;

    [Header("Comet Configs")]
    [SerializeField] private CometConfigSO _cometConfigSO;

    [Header("Default Comet")]
    [SerializeField] private Transform _defaultCometTransform;

    [Header("Ice Comet")]
    [SerializeField] private Transform _iceCometTransform;
    private float _iceCometSpawnProbability;

    [Header("Electro Comet")]
    [SerializeField] private Transform _electroCometTransform;

    [SerializeField] private float _delayBetweenDestruction = 0.1f;
    private float _electroCometSpawnProbability;
    
    

    public List<Transform> ActiveComets = new List<Transform>();

    private void Awake()
    {
        _iceCometSpawnProbability = _cometConfigSO.IceCometSpawnProbabilty;
        _electroCometSpawnProbability = _cometConfigSO.ElectroCometSpawnProbabilty;
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

        if (Random.value < _electroCometSpawnProbability)
        {
            return Instantiate(_electroCometTransform);
        }

        return Instantiate(_defaultCometTransform);
    }

    private void HandleCometHit(CometType type, GameObject hitObject)
    {
        switch (type)
        {
            case CometType.Ice:
                FreezeAllActiveComets();
                break;
            case CometType.Electro:
                StartCoroutine(CauseChainLightning(hitObject));
                
                break;
        }
    }

    private void FreezeAllActiveComets()
    {
        var activeComets = GetActiveComets();

        foreach (var comet in activeComets)
        {
            var obj = comet.GetComponent<Comet>();
            obj.Freeze();
            obj.SetMaterial(_cometConfigSO.IceCometMaterial);
        }
    }

    IEnumerator CauseChainLightning(GameObject hitObject)
    {
        List<GameObject> destroyableObjects = GameObject.FindGameObjectsWithTag("Comet").ToList();

        destroyableObjects.Remove(hitObject);

        GameObject currentObject = hitObject;

        var sortedDestroyableObjects = destroyableObjects
            .OrderBy(obj => Vector3.Distance(currentObject.transform.position, obj.transform.position));

        foreach (var obj in sortedDestroyableObjects)
        {
          obj.GetComponent<Comet>().Electricute();
          yield return new WaitForSeconds(_delayBetweenDestruction);
        }
    }

    private List<Transform> GetActiveComets()
    {
        return ActiveComets.Where(comet => !comet.IsDestroyed()).ToList();
    }
}
