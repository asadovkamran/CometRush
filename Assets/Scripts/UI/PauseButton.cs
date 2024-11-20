using UnityEngine;
using UnityEngine.UI;


public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameStatsSO _gameStatsSo;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.image.sprite = _pauseSprite;
        _gameStatsSo.GamePausedEvent.AddListener(HandleGamePausedEvent);
    }

    private void OnDisable()
    {
        _gameStatsSo.GamePausedEvent.RemoveListener(HandleGamePausedEvent);
    }

    private void HandleGamePausedEvent()
    {
        ToggleIcon();
    }

    public void ToggleIcon()
    {
        _button.image.sprite = _button.image.sprite == _pauseSprite ? _playSprite : _pauseSprite;
    }
}
