using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private GameStatsSO _gameStatsSO;

    private void OnEnable()
    {
        Initialize();
        _gameStatsSO.CurrentHealthChangeEvent.AddListener(HandleHealthChangeEvent);
    }

    private void OnDisable()
    {
        _gameStatsSO.CurrentHealthChangeEvent.RemoveListener(HandleHealthChangeEvent);
    }

    private void Initialize()
    {
        HandleHealthChangeEvent(_gameStatsSO.CurrentHealth);
    }

    private void HandleHealthChangeEvent(float health)
    {
        _healthSlider.value = Mathf.Clamp01(health / 100);
    }
}
