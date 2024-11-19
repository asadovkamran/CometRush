using UnityEngine;
using CometRush.Enums;
using System;

public class Comet : MonoBehaviour
{
    public GameConstants GAME_CONSTANTS;
    public float Speed;
    public CometType Type;
    public float CometDamage;
    public Mesh[] Meshes;
    public GameObject[] Explosions;

    public static event Action<float> OnCometReachPlayer;

    [SerializeField] private GameStatsSO _gameStatsSO;
    [SerializeField] private GameOverSO _gameOverSO;
    private Vector3 _cometTarget;
    private Rigidbody _rb;
    private MeshFilter _filter;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        Initialize();

        HandleSpawn();
        HandlePushTowardsPlayer();
    }

    private void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        _filter = GetComponent<MeshFilter>();

        _sphereCollider = gameObject.AddComponent<SphereCollider>();
        _sphereCollider.radius *= GAME_CONSTANTS.COMET_COLLIDER_RADIUS_SCALE;

        Mesh currentMesh = Meshes[UnityEngine.Random.Range(0, Meshes.Length)];
        _filter.mesh = currentMesh;
    }

    private void FixedUpdate()
    {
        if (_rb.position.z > Camera.main.transform.position.z)
        {
            OnCometReachPlayer?.Invoke(CometDamage);
            HandleCometReachPlayer();
            Destroy(gameObject);
        }
    }

    private void HandleCometReachPlayer()
    {
        if (_gameStatsSO.CurrentShields < CometDamage)
        {
            _gameStatsSO.CurrentHealth = Mathf.Clamp(_gameStatsSO.CurrentHealth - (CometDamage - _gameStatsSO.CurrentShields), 0, _gameStatsSO.GetMaxHealth());
            _gameStatsSO.CurrentShields = 0;
        }
        else
        {
            _gameStatsSO.CurrentShields = Mathf.Clamp(_gameStatsSO.CurrentShields - CometDamage, 0, _gameStatsSO.GetMaxShields());
        }

        _gameStatsSO.UpdateHealth();
        _gameStatsSO.UpdateShields();

        if (_gameStatsSO.CurrentHealth <= 0)
        {
            _gameOverSO.OnPlayerDead();
        }
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += HandleHit;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= HandleHit;
    }

    private void HandleSpawn()
    {
        float spawnDistance = UnityEngine.Random.Range(GAME_CONSTANTS.MIN_SPAWN_DISTANCE, GAME_CONSTANTS.MAX_SPAWN_DISTANCE);

        int horizontalPaddingPx = (int)(Camera.main.pixelWidth * GAME_CONSTANTS.COMET_SPAWN_HORIZONTAL_PADDING_PERCENT / 100);
        int verticalPaddingPx = (int)(Camera.main.pixelHeight * GAME_CONSTANTS.COMET_SPAWN_VERTICAL_PADDING_PERCENT / 100);

        Vector3 spawnPositionScreenPoint = new Vector3(
            UnityEngine.Random.Range(horizontalPaddingPx, Camera.main.pixelWidth - horizontalPaddingPx),
            UnityEngine.Random.Range(verticalPaddingPx, Camera.main.pixelHeight - verticalPaddingPx),
            spawnDistance
        );
        Vector3 spawnPositionWorld = Camera.main.ScreenToWorldPoint(spawnPositionScreenPoint);
        _rb.position = spawnPositionWorld;
    }

    private void HandlePushTowardsPlayer()
    {
        Vector3 randomTargetPosition = new Vector3(
            UnityEngine.Random.Range(0, Camera.main.pixelWidth),
            UnityEngine.Random.Range(0, Camera.main.pixelHeight),
            -UnityEngine.Random.Range(1, 5)
        );
        _cometTarget = Camera.main.ScreenToWorldPoint(randomTargetPosition);

        Speed = GAME_CONSTANTS.COMET_BASE_SPEED + UnityEngine.Random.Range(-GAME_CONSTANTS.COMET_SPEED_VARIANCE, GAME_CONSTANTS.COMET_SPEED_VARIANCE) + GameManager.Instance.getDifficulty();

        Vector3 force = (_cometTarget - _rb.position).normalized * Speed;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    private void HandleHit(GameObject obj)
    {
        if (obj != null && GameObject.ReferenceEquals(obj, gameObject))
        {
            Instantiate(Explosions[UnityEngine.Random.Range(0, Explosions.Length)], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
