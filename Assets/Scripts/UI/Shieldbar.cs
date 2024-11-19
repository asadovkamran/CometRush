using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shieldbar : MonoBehaviour
{
    [SerializeField] private Slider _shieldsSlider;
    [SerializeField] private GameStatsSO _gameStatsSO;

    private void OnEnable()
    {
        Initialize();
        _gameStatsSO.CurrentShieldsChangeEvent.AddListener(HandleShieldsChangeEvent);
    }

    private void OnDisable()
    {
        _gameStatsSO.CurrentShieldsChangeEvent.RemoveListener(HandleShieldsChangeEvent);
    }

    private void Initialize()
    {
        HandleShieldsChangeEvent(_gameStatsSO.CurrentShields);
    }

    private void HandleShieldsChangeEvent(float shields)
    {
        _shieldsSlider.value = Mathf.Clamp01(shields / 100);
    }
}
