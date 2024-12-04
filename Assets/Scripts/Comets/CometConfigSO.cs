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
    public Color DefaultTextColor = Color.white;
    public int DefaultCometPoints = 1;

    [Header("Ice Comet Configs")]
    public Transform IceCometTransform;
    public Material IceCometMaterial;
    public GameObject IceCometExplosion;
    public int IceCometPoints = 2;
    public Color IceTextColor = Color.cyan;
    public float IceCometSpawnProbabilty = 0.05f;

    [Header("Electro Comet Configs")]
    public Transform ElectroCometTransform;
    public Material ElectroCometMaterial;
    public GameObject ElectroCometExplosion;
    public int ElectroCometPoints = 1;
    public Color ElectroTextColor = Color.magenta;
    public float ElectroCometSpawnProbabilty = 0.025f;

    public void OnCometHit(CometType type,GameObject hitObject)
    {
        CometHitEvent?.Invoke(type, hitObject);
    }
}
