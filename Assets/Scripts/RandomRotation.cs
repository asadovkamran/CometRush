using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        transform.rotation = Random.rotation;
        rb.angularVelocity = Random.onUnitSphere * Random.Range(2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
