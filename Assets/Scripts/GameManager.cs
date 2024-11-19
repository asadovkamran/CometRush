using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


//todo: get rid of global non-readonly state
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameStatsSO _gameStatsSO;
    [SerializeField] private GameOverSO _gameOverSO;
    [SerializeField] private float _difficulty = 0;

    public static GameManager Instance;
    public GameConstants GAME_CONSTANTS;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("RegenerateShields", 2f, GAME_CONSTANTS.SHIELD_REGEN_RATE);
    }

    private void OnEnable()
    {
        HealthRestoreItem.OnHealthPickUp += HandleOnHealthPickUp;
    }

    private void OnDisable()
    {
        HealthRestoreItem.OnHealthPickUp -= HandleOnHealthPickUp;
    }

    private void Update()
    {
        _gameStatsSO.ElapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void HandleOnCometReachPlayer(float damageAmount)
    {
        if (_gameStatsSO.CurrentHealth <= 0)
        {
            _gameOverSO.OnPlayerDead();
        }

        if (_gameStatsSO.CurrentShields < damageAmount)
        {
            _gameStatsSO.CurrentHealth = Mathf.Clamp(_gameStatsSO.CurrentHealth - (damageAmount - _gameStatsSO.CurrentShields), 0, _gameStatsSO.GetMaxHealth());
            _gameStatsSO.CurrentShields = 0;
        }
        else
        {
            _gameStatsSO.CurrentShields = Mathf.Clamp(_gameStatsSO.CurrentShields - damageAmount, 0, _gameStatsSO.GetMaxShields());
        }

        _gameStatsSO.UpdateHealth();
        _gameStatsSO.UpdateShields();
    }

    private void HandleOnHealthPickUp()
    {
       
        if (_gameStatsSO.CurrentHealth < GAME_CONSTANTS.PLAYER_MAX_HEALTH)
        {
            _gameStatsSO.CurrentHealth = Mathf.Clamp(_gameStatsSO.CurrentHealth + GAME_CONSTANTS.HEALTH_ITEM_HEAL_AMOUNT, 0, GAME_CONSTANTS.PLAYER_MAX_HEALTH);
            _gameStatsSO.UpdateHealth();
        }
    }

    private void RegenerateShields()
    {
        _gameStatsSO.CurrentShields = Mathf.Clamp(_gameStatsSO.CurrentShields + 1, 0f, 100f);
        _gameStatsSO.UpdateShields();
    }

    public float getDifficulty()
    {
        _difficulty += Time.deltaTime * GAME_CONSTANTS.DIFFICULTY_COEFFICIENT;
        return _difficulty;
    }
}
