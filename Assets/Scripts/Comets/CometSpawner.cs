using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CometRush.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    [SerializeField] private CometsPool _pool; 
    [Header("Game Constants")]
    [SerializeField] private GameConstants GAME_CONSTANTS;

    [SerializeField] private GameStatsSO _gameStatsSO;

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
            ActiveComets = ActiveComets.Where(comet => comet.gameObject.activeSelf).ToList();

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
            return _pool.GetObject("IceComet").transform;
        }

        if (Random.value < _electroCometSpawnProbability)
        {
            return _pool.GetObject("ElectroComet").transform;
        }

        return _pool.GetObject("DefaultComet").transform;
    }

    private void HandleCometHit(CometType type, GameObject hitObject)
    {
        int points = 1;
        _gameStatsSO.AddScore(points);
        // todo: show floating text here
        switch (type)
        {
            case CometType.Default:
                break;
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
        //todo: get rid of GameObject.Find
        List<Transform> destroyableObjects = ActiveComets;

        destroyableObjects.Remove(hitObject.transform);

        GameObject currentObject = hitObject;

        var sortedDestroyableObjects = destroyableObjects
            .OrderBy(obj => Vector3.Distance(currentObject.transform.position, obj.transform.position));

        List<Vector3> positions = new List<Vector3>();
        positions.Add(hitObject.gameObject.transform.position);
        foreach (var obj in sortedDestroyableObjects)
        {
            positions.Add(obj.gameObject.transform.position);
        }

        LineRenderer lineRenderer = CreateLineRenderer(positions);
        List<Vector3> linePositions = new List<Vector3> { currentObject.transform.position };

        int chainLightningStreak = 1;
        foreach (var obj in sortedDestroyableObjects)
        {
            _gameStatsSO.AddScore(chainLightningStreak++);
            obj.GetComponent<Comet>().Electricute();
          //todo: show floating text
          yield return new WaitForSeconds(_delayBetweenDestruction);
        }
    }

    LineRenderer CreateLineRenderer(List<Vector3> positions)
    {
        GameObject lineObject = new GameObject("ChainLightningLine");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
     
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.white;

        DestroyTimer destroyTimer = lineObject.AddComponent<DestroyTimer>();
        destroyTimer.Timer = 0.1f;

        return lineRenderer;
    }

    private List<Transform> GetActiveComets()
    {
        return ActiveComets.Where(comet => !comet.IsDestroyed()).ToList();
    }
}
