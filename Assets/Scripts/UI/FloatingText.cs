using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private float _scaleAmount;
    [SerializeField] private float _scaleTime;
    [SerializeField] private LeanTweenType _scaleTweenType;
    public TextMeshProUGUI FloatingTextObj;
    [SerializeField] private ObjectPool _floatingTextPool;
    private void Awake()
    {
        FloatingTextObj.transform.localScale = Vector3.zero;
        _floatingTextPool = GameObject.Find("FloatingTextPool").GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        
        FloatingTextObj.transform.LeanScale(new Vector3(_scaleAmount, _scaleAmount, _scaleAmount), _scaleTime)
            .setEase(_scaleTweenType).setOnComplete(() => {
                _floatingTextPool.ReturnToPool(gameObject);
            });
    }

    private void OnDisable()
    {
        ResetFloatingText();
    }

    private void ResetFloatingText()
    {
        FloatingTextObj.transform.localScale = Vector3.zero;
        FloatingTextObj.text = "";
        FloatingTextObj.color = Color.white;
        FloatingTextObj.rectTransform.position = Vector3.zero;  
    }
}
