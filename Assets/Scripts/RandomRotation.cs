using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.rotation = Random.rotation;
        _rb.angularVelocity = Random.onUnitSphere * Random.Range(2.0f, 5.0f);
    }
}
