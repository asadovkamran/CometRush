using CometRush.Enums;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CometConfigSO", menuName = "Scriptable Objects/CometConfigSO")]
public class CometConfigSO : ScriptableObject
{
    public UnityEvent<CometType> CometHitEvent;

    [Header("Default Comet Configs")]
    public Transform DefaultCometTransform;
    public Material DefaultCometMaterial;
    public GameObject DefaultCometExplosion;

    [Header("Ice Comet Configs")]
    public Transform IceCometTransform;
    public Material IceCometMaterial;
    public GameObject IceCometExplosion;
    public float IceCometSpawnProbabilty = 0.05f;

    private void Awake()
    {

    }

    public void OnCometHit(CometType type)
    {
        CometHitEvent?.Invoke(type);
    }
}
