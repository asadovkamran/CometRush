using CometRush.Enums;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CometConfigSO", menuName = "Scriptable Objects/CometConfigSO")]
public class CometConfigSO : ScriptableObject
{
    public UnityEvent<CometType, GameObject> CometHitEvent;

    [Header("Default Comet Configs")]
    public Transform DefaultCometTransform;
    public Material DefaultCometMaterial;
    public GameObject DefaultCometExplosion;

    [Header("Ice Comet Configs")]
    public Transform IceCometTransform;
    public Material IceCometMaterial;
    public GameObject IceCometExplosion;
    public float IceCometSpawnProbabilty = 0.05f;

    [Header("Electro Comet Configs")]
    public Transform ElectroCometTransform;
    public Material ElectroCometMaterial;
    public GameObject ElectroCometExplosion;
    public float ElectroCometSpawnProbabilty = 0.025f;

    public void OnCometHit(CometType type,GameObject hitObject)
    {
        CometHitEvent?.Invoke(type, hitObject);
    }
}
