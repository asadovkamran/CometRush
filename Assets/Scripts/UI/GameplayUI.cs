using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeReference] private TextMeshProUGUI _timerText;
    [SerializeField] private GameStatsSO _gameStatsSO; 
    [SerializeField] private GameConstants _gameConstants;

    [Header("Ability UI")]
    
    [SerializeField] private Button _abilityButton;
    [SerializeField] private Sprite _abilityButtonDefaultSprite;
    [SerializeField] private Sprite _abilityButtonReadySprite;
    [SerializeField] private Image _cooldownOverlay;
    private bool _onCoolDown = true;
    private float _abilityCooldown = 5f;

    private float _currentHitAmount = 0;

    public static event Action OnAbilityUsed;

    private void Start()
    {
        UpdateScoreText(_gameStatsSO.Score);
    }

    private void Update()
    {
        UpdateTimer();   
    }

    private void OnEnable()
    {
        UpdateScoreText(_gameStatsSO.Score);
        _gameStatsSO.ScoreChangeEvent.AddListener(UpdateScoreText);
        HitDetection.OnCometHit += HandleCometHit;
    }

    private void OnDisable()
    {
        _gameStatsSO.ScoreChangeEvent?.RemoveListener(UpdateScoreText);
        HitDetection.OnCometHit -= HandleCometHit;
    }

    private void UpdateScoreText(float value)
    {
        _scoreText.text = "Score: " + value;   
    }

    private void UpdateTimer()
    {
        float elapsedTime = _gameStatsSO.ElapsedTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);

        _timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void OnAbilityButtonClick()
    {
        if (!_onCoolDown) {
            OnAbilityUsed?.Invoke();
            _onCoolDown = true;
            ResetCooldownOverlay();
            ResetAbilityButton();
            _abilityButton.image.sprite = _abilityButtonDefaultSprite;
        }
    }

    private void HandleCometHit(GameObject comet)
    {
        if (_onCoolDown) _currentHitAmount += _gameConstants.FREEZE_ABILITY_REPLENISH_RATE;
        
        UpdateCooldownOverlay();
        if (_currentHitAmount >= _gameConstants.FREEZE_ABILITY_MAX_CAPACITY)
        {
            _onCoolDown = false;
            _currentHitAmount = 0;
            _abilityButton.image.sprite = _abilityButtonReadySprite;
            AnimateAbilityButton();
        }
    }

    private void UpdateCooldownOverlay()
    {
        _cooldownOverlay.fillAmount -= _gameConstants.FREEZE_ABILITY_REPLENISH_RATE / _gameConstants.FREEZE_ABILITY_MAX_CAPACITY;
    }

    private void ResetCooldownOverlay()
    {
        _cooldownOverlay.fillAmount = 1;
    }

    private void AnimateAbilityButton()
    {
        _abilityButton.gameObject.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).setEaseInOutCubic().setLoopPingPong();
    }   

    private void ResetAbilityButton()
    {
        _abilityButton.gameObject.LeanScale(new Vector3(1f, 1f, 1f), 0.1f).setEaseInOutCubic().setOnComplete(() => _abilityButton.gameObject.LeanCancel());
    }
}

