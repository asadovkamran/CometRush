using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private float _scaleAmount;
    [SerializeField] private float _scaleTime;
    [SerializeField] private LeanTweenType _scaleTweenType;
    public TextMeshProUGUI FloatingTextObj;

    private void Awake()
    {
        FloatingTextObj.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        
        FloatingTextObj.transform.LeanScale(new Vector3(_scaleAmount, _scaleAmount, _scaleAmount), _scaleTime)
            .setEase(_scaleTweenType);
    }
}
