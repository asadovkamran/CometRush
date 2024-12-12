using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private float _lastHitTime;
    private TextMeshProUGUI _comboText;
    private TextMeshProUGUI _scoreToAddText;
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
        _scoreToAddText = GameObject.Find("ScoreToAddText").GetComponent<TextMeshProUGUI>();
        _comboText.text = "";
        _scoreToAddText.text = "";
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
            HideScoreToAddText();
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

    public void AddPoints( GameObject comet, int chainLightningStreak = 0) {
        var cometPoints = comet.GetComponent<Comet>().GetCometPoints();

        if (ComboActive)
        {
            _comboCount++;     
        }
        else
        {
            _comboCount = 1;
        }

        _pointsCount += cometPoints + chainLightningStreak;

        ShowScoreToAddText();
        _lastHitTime = Time.time;
    }

    private void ShowComboText()
    {
        UpdateText(_comboText, _comboCount);
    }

    private void ShowScoreToAddText() {
        UpdateText(_scoreToAddText, _pointsCount * _comboCount);
    }

    private void UpdateText(TextMeshProUGUI textMesh, int value)
    {
        textMesh.color = _comboTextColorDefault;

        if (_comboCount > 3) {
            textMesh.color = _comboTextColorYellow;
        }

        if (_comboCount > 6) {
            textMesh.color = _comboTextColorOrange;
        }

        if (_comboCount > 9) {
            textMesh.color = _comboTextColorRed;
        }

        textMesh.text = (textMesh == _comboText ? "x" : "+") + value;
        //textMesh.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        textMesh.gameObject.LeanScale(textMesh.gameObject.transform.localScale + new Vector3(1.1f, 1.1f, 1.1f), 0.2f)
        .setEasePunch().setOnComplete(() =>
        {
            textMesh.gameObject.LeanScale(new Vector3(1f, 1f, 1f), 0.1f).setEaseInOutCubic();
        });
    }

    private void HideScoreToAddText() {
        _scoreToAddText.text = "";
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
