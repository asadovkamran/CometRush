using CometRush.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float speed;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float spawnDistance = Random.Range(20, 100);

        Vector3 randomSpawnPosition = new Vector3(Random.Range(0, Camera.main.pixelWidth),
            Random.Range(0, Camera.main.pixelHeight), spawnDistance);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(randomSpawnPosition);


        rb.position = worldPosition;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed * Time.fixedDeltaTime;
        rb.AddForce(rb.velocity, ForceMode.Force);

        if (rb.position.z > Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}