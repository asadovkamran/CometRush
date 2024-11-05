using System.Collections;
using UnityEngine;

public class ShakeableTransform : MonoBehaviour
{
    public static ShakeableTransform Instance;

    [SerializeField] private float _frequency = 25;
    [SerializeField] private Vector3 _maximumTranslationShake = Vector3.one * 0.5f;
    [SerializeField] private float _recoverySpeed = 1.5f;
    [SerializeField] private float _hurtFadeOutTime = 0.5f;
    [SerializeField] private float _traumaExponent = 2;
    private float _trauma = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void RunShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float shake = Mathf.Pow(_trauma, _traumaExponent);
            transform.localPosition = new Vector3(
               _maximumTranslationShake.x * Mathf.PerlinNoise(0, Time.time * _frequency) * 2 - 1,
               _maximumTranslationShake.y * Mathf.PerlinNoise(1, Time.time * _frequency) * 2 - 1,
               _maximumTranslationShake.z * Mathf.PerlinNoise(2, Time.time * _frequency) * 2 - 1
            ) * shake;

            yield return null;
        }
    }
}
