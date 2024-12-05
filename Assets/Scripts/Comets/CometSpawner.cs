using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CometRush.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CometSpawner : MonoBehaviour
{
    [SerializeField] private CometsPool _pool;
    [SerializeField] private CometsPool _explosionsPool;
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

    [SerializeField] private GameObject _floatingText;

    public List<Transform> ActiveComets = new List<Transform>();

    private void Awake()
    {
        _iceCometSpawnProbability = _cometConfigSO.IceCometSpawnProbabilty;
        _electroCometSpawnProbability = _cometConfigSO.ElectroCometSpawnProbabilty;
    }

    private void OnEnable()
    {
        GameplayUI.OnAbilityUsed += HandleAbilityUsed;
        _cometConfigSO.CometHitEvent.AddListener(HandleCometHit);
    }

    private void OnDisable()
    {
        _cometConfigSO.CometHitEvent.RemoveListener(HandleCometHit);
        GameplayUI.OnAbilityUsed -= HandleAbilityUsed;
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
        if (Random.value < _electroCometSpawnProbability)
        {
            return _pool.GetObject(CometType.Electro).transform;
        }

        return _pool.GetObject(CometType.Default).transform;
    }

    private void MakeExplosion(CometType type, Vector3 position)
    {
        GameObject explosion = _explosionsPool.GetObject(type);
        explosion.transform.position = position;
        explosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(ExplosionReturnToPool(type, explosion));
    }

    IEnumerator ExplosionReturnToPool(CometType type, GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        _explosionsPool.ReturnToPool(type, obj);
    }

    private void HandleCometHit(CometType type, GameObject hitObject)
    {
        Comet comet = hitObject.GetComponent<Comet>();
        Color textColor = comet.GetTextColor();

        _gameStatsSO.AddScore(comet.GetCometPoints());
        ShowFloatingText(hitObject.transform.position, textColor, comet.GetCometPoints().ToString());
        MakeExplosion(type, hitObject.transform.position);

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

    private void ShowFloatingText(Vector3 position, Color textColor, string text = "1")
    {
        GameObject floatingTextObj = Instantiate(_floatingText, Vector3.zero, Quaternion.identity);
        TextMeshProUGUI textObj = floatingTextObj.GetComponent<FloatingText>().FloatingTextObj;
        textObj.text = "+" + text;
        textObj.color = textColor;
        textObj.rectTransform.position = Camera.main.WorldToScreenPoint(position) + new Vector3(0,80f,0);
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
            chainLightningStreak++;
            _gameStatsSO.AddScore(chainLightningStreak);
            ShowFloatingText(obj.transform.position, Color.magenta, chainLightningStreak.ToString());

            MakeExplosion(CometType.Electro, obj.transform.position);
            
            obj.GetComponent<Comet>().Electricute();
            
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

    private void HandleAbilityUsed()
    {
        FreezeAllActiveComets();
    }
}
