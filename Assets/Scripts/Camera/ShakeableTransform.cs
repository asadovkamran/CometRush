using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableTransform : MonoBehaviour
{
    public static ShakeableTransform Instance;
    [SerializeField] float frequency = 25;
    [SerializeField] Vector3 maximumTranslationShake = Vector3.one * 0.5f;
    [SerializeField] float recoverySpeed = 1.5f;
    [SerializeField] private float hurtFadeOutTime = 0.5f;

    private float trauma = 1;

    [SerializeField] float traumaExponent = 2;

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
        while (elapsedTime < hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float shake = Mathf.Pow(trauma, traumaExponent);
            transform.localPosition = new Vector3(
               maximumTranslationShake.x * Mathf.PerlinNoise(0, Time.time * frequency) * 2 - 1,
               maximumTranslationShake.y * Mathf.PerlinNoise(1, Time.time * frequency) * 2 - 1,
               maximumTranslationShake.z * Mathf.PerlinNoise(2, Time.time * frequency) * 2 - 1
            ) * shake;

            yield return null;
        }
    }
}
