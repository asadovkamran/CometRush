using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RandomRotation : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    private void Start()
    {
        transform.rotation = Random.rotation;
        _rb.angularVelocity = Random.onUnitSphere * Random.Range(2.0f, 5.0f);
    }

    private void OnEnable()
    {
        SetRandomRotation();
    }

    private void SetRandomRotation()
    {
        transform.rotation = Random.rotation;
        _rb.angularVelocity = Random.onUnitSphere * Random.Range(2.0f, 5.0f);
    }

    private void OnValidate()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
