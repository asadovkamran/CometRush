using System;
using System.Collections;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeReference] private TextMeshProUGUI _timerText;
    [SerializeField] private GameStatsSO _gameStatsSO; 
    [SerializeField] private GameConstants _gameConstants;

    [SerializeField] private TextMeshProUGUI _abilityCooldownText;
    private bool _onCoolDown = true;
    private float _abilityCooldown = 5f;

    private float _currentHitAmount = 0;

    public static event Action OnAbilityUsed;

    private void Start()
    {
        _abilityCooldownText.text = $"{_abilityCooldown:0}s";
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
        }
    }

    IEnumerator AbilityCooldown(float currentCooldown)
    {
        _onCoolDown = true;
        float cooldownTimeLeft = currentCooldown;

        while (cooldownTimeLeft > 0)
        {
            UpdateAbilityButtonText(cooldownTimeLeft);
            cooldownTimeLeft -= Time.deltaTime;
            yield return null;
        }

        UpdateAbilityButtonText(0);
        _onCoolDown = false;
    }

    private void UpdateAbilityButtonText(float cooldown)
    {
        _abilityCooldownText.text = cooldown > 0 ? $"{cooldown:0}s" : "Freeze!";
    }

    private void HandleCometHit(GameObject comet)
    {
        _currentHitAmount += _gameConstants.FREEZE_ABILITY_REPLENISH_RATE;
        if (_currentHitAmount >= _gameConstants.FREEZE_ABILITY_MAX_CAPACITY)
        {
            _onCoolDown = false;
            _currentHitAmount = 0;
        }
    }
}
