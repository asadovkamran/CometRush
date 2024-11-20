using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class FullScreenDamage : MonoBehaviour
{
    [SerializeField] private GameStatsSO _gameStatsSO;

    [Header("Time Stats")]
    [SerializeField] private float _hurtDisplayTime = 1.5f;
    [SerializeField] private float _hurtFadeOutTime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenDamage;
    [SerializeField] private Material _material;

    [Header("Colors")]
    [SerializeField] private Color _shieldMaterialColor;
    [SerializeField] private Color _healthMaterialColor;

    [Header("Intensity Stats")]
    [SerializeField] private float _voronoiIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.25f;

    private int _voronoiIntensity = Shader.PropertyToID("_VoronoiIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    private void Start()
    {
        _material.color = _shieldMaterialColor;
        _fullScreenDamage.SetActive(false);
    }

    private void OnEnable()
    {
        Comet.OnCometReachPlayer += HandleOnCometReachPlayer;
    }

    private void OnDisable()
    {
        Comet.OnCometReachPlayer -= HandleOnCometReachPlayer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Hurt());
        }
    }

    private void HandleOnCometReachPlayer(float damage)
    {
        _material.color = _gameStatsSO.CurrentShields > damage ? _shieldMaterialColor : _healthMaterialColor;
        RunHurt();
    }

    public void RunHurt()
    {
        StartCoroutine(Hurt());
    }

    private IEnumerator Hurt()
    {
        _fullScreenDamage.SetActive(true);
        _material.SetFloat(_voronoiIntensity, _voronoiIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);

        yield return new WaitForSeconds(_hurtDisplayTime);

        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedVoronoi = Mathf.Lerp(_voronoiIntensityStat, 0f, (elapsedTime / _hurtFadeOutTime));
            float lerpedVignette = Mathf.Lerp(_vignetteIntensityStat, 0f, (elapsedTime / _hurtFadeOutTime));

            _material.SetFloat(_voronoiIntensity, lerpedVoronoi);
            _material.SetFloat(_vignetteIntensity, lerpedVignette);

            yield return null;
        }

        _fullScreenDamage.SetActive(false);
    }
}
