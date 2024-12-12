using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private float _lastHitTime;
    private TextMeshProUGUI _comboText;
    private int _pointsCount = 0;
    private Color _comboTextColorDefault = Color.white;
    private Color _comboTextColorYellow = Color.yellow;
    private Color _comboTextColorOrange = new Color(1, 0.5f, 0);
    private Color _comboTextColorRed = new Color(1, 0, 0);
    private float _comboDuration = 0.5f;
    [SerializeField] private GameStatsSO _gameStatsSO;
    private int _comboCount = 0;


    private void Awake()
    {
        _lastHitTime = Time.time;
        _comboText = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
        _comboText.text = "";
    }

    private void OnEnable()
    {
        HitDetection.OnCometHit += HandleCometHit;
    }

    private void OnDisable()
    {
        HitDetection.OnCometHit -= HandleCometHit;
    }

    private void Start()
    {
        InvokeRepeating("ResetComboIfInactive", 0f, 0.1f);
    }

    private void ResetComboIfInactive()
    {
        if (!ComboActive)
        {
            HideComboText();
            AddScore();
            _comboCount = 0;
            _pointsCount = 0;
            
        }
    }

    private bool ComboActive => Time.time - _lastHitTime <= _comboDuration;

    private void HandleCometHit(GameObject comet)
    {
        AddPoints(comet);
    }

    public void AddPoints( GameObject comet) {
        var cometPoints = comet.GetComponent<Comet>().GetCometPoints();

        if (ComboActive)
        {
            _comboCount++;     
        }
        else
        {
            _comboCount = 1;
        }

        _pointsCount += cometPoints;

        ShowComboText();
        _lastHitTime = Time.time;
    }

    private void ShowComboText()
    {
        _comboText.color = _comboTextColorDefault;

        if (_comboCount > 3) {
            _comboText.color = _comboTextColorYellow;
        }

        if (_comboCount > 6) {
            _comboText.color = _comboTextColorOrange;
        }

        if (_comboCount > 9) {
            _comboText.color = _comboTextColorRed;
        }

   _comboText.text = "x" + _comboCount;
        _comboText.gameObject.LeanScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f).setEasePunch().setOnComplete(() =>
        {
            _comboText.gameObject.LeanScale(new Vector3(1f, 1f, 1f), 0.2f).setEaseInOutCubic();
        });
    }

    private void HideComboText()
    {
        _comboText.text = "";
    }

    private void AddScore()
    {
        _gameStatsSO.AddScore(_pointsCount * _comboCount);
    }   
}
