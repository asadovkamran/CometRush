using UnityEngine;
using CometRush.Enums;
using System;
using RDG;
using UnityEngine.Events;

public class Comet : MonoBehaviour
{
    public GameConstants GAME_CONSTANTS;
    public float Speed;
    public CometType Type;
    public float CometDamage;
    public Mesh[] Meshes;

    public static event Action<float> OnCometReachPlayer;
    public static event Action<GameObject> OnCometHitEvent;

    [SerializeField] private CometsPool _cometsPool;
    [SerializeField] private GameStatsSO _gameStatsSO;
    [SerializeField] private GameOverSO _gameOverSO;
    [SerializeField] private CometConfigSO _cometConfigSO;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _material;

    private Vector3 _cometTarget;
    private Rigidbody _rb;
    private MeshFilter _filter;
    private SphereCollider _sphereCollider;
    private bool isElectricuted = false;
    private Material _material_tmp;
    private float _speed_tmp;
    private CometType _cometStatus;
    private int _cometPoints;
    private Color _textColor;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        _speed_tmp = Speed;
        _cometsPool = GameObject.Find("CometsPool").GetComponent<CometsPool>();
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += HandleHit;
        Speed = _speed_tmp;
        SetMaterial(_material);
        HandleSpawn();
        HandlePushTowardsPlayer();
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= HandleHit;
        ResetComet();
    }

    private void FixedUpdate()
    {
        if (_rb.position.z > Camera.main.transform.position.z)
        {
            OnCometReachPlayer?.Invoke(CometDamage);
            HandleCometReachPlayer();
            ReturnToPool();
        }
    }

    private void Initialize()
    {

        _rb = GetComponent<Rigidbody>();
        _filter = GetComponent<MeshFilter>();

        _sphereCollider = gameObject.AddComponent<SphereCollider>();
        _sphereCollider.radius *= GAME_CONSTANTS.COMET_COLLIDER_RADIUS_SCALE;

        Mesh currentMesh = Meshes[UnityEngine.Random.Range(0, Meshes.Length)];
        _filter.mesh = currentMesh;

        switch (Type)
        {
            case CometType.Default:
                _cometPoints = _cometConfigSO.DefaultCometPoints;
                _textColor = _cometConfigSO.DefaultTextColor;
                break;
            case CometType.Ice:
                _cometPoints = _cometConfigSO.IceCometPoints;
                _textColor = _cometConfigSO.IceTextColor;
                break;
            case CometType.Electro:
                _cometPoints = _cometConfigSO.ElectroCometPoints;
                _textColor = _cometConfigSO.ElectroTextColor;
                break;
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
        if (obj != null && GameObject.ReferenceEquals(obj, gameObject) && !isElectricuted)
        {
            _cometConfigSO.OnCometHit(Type, obj);
            Destroy();
        }
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public void Destroy()
    {
        // vibrate android device
        Vibration.Vibrate(25, 255);
        OnCometHitEvent?.Invoke(gameObject);
        ReturnToPool();
    }

    public void Freeze()
    {
        _rb.velocity = Vector3.zero;
        _cometStatus = CometType.Ice;
        _cometPoints = _cometConfigSO.IceCometPoints;
        _textColor = _cometConfigSO.IceTextColor;
    }

    public CometType GetCometStatus()
    {
        return _cometStatus;
    }

    public void Electricute()
    {
        isElectricuted = true;
        SetMaterial(_cometConfigSO.ElectroCometMaterial);
        Destroy();
    }

    private void ReturnToPool()
    {
        switch (Type)
        {
            case CometType.Default:
                _cometsPool.ReturnToPool(CometType.Default, gameObject);
                break;
            case CometType.Ice:
                _cometsPool.ReturnToPool(CometType.Ice, gameObject);
                break;
            case CometType.Electro:
                _cometsPool.ReturnToPool(CometType.Electro, gameObject);
                break;
        }

    }

    private void ResetComet()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();

        _rb.velocity = Vector3.zero;
        Speed = 0;
        _rb.angularVelocity = Vector3.zero;
        _rb.position = new Vector3(0, 0, -1000);
        transform.position = _rb.position;

        _meshRenderer.material = _material_tmp;

        isElectricuted = false;

        switch (Type)
        {
            case CometType.Default:
                _cometPoints = _cometConfigSO.DefaultCometPoints;
                _textColor = _cometConfigSO.DefaultTextColor;
                break;
            case CometType.Ice:
                _cometPoints = _cometConfigSO.IceCometPoints;
                _textColor = _cometConfigSO.IceTextColor;
                break;
            case CometType.Electro:
                _cometPoints = _cometConfigSO.ElectroCometPoints;
                _textColor = _cometConfigSO.ElectroTextColor;
                break;
        }

    }

    public int GetCometPoints()
    {
        return _cometPoints;
    }

    public Color GetTextColor()
    {
        return _textColor;
    }
}
