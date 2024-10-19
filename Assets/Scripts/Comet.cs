using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CometRush.Enums;

public class Comet : MonoBehaviour
{
    public float speed;
    public CometType type;
    private Vector3 cometTarget;

    public GameObject[] explosion;
    
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float spawnDistance = Random.Range(20, 100);

        Vector3 randomSpawnPosition = new Vector3(Random.Range(0, Camera.main.pixelWidth),
            Random.Range(0, Camera.main.pixelHeight), spawnDistance);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(randomSpawnPosition);

        Vector3 randomTargetPosition = new Vector3(Random.Range(0, Camera.main.pixelWidth),
            Random.Range(0, Camera.main.pixelHeight), -Random.Range(1, 5));
        cometTarget = Camera.main.ScreenToWorldPoint(randomTargetPosition);

        rb.position = worldPosition;

        Vector3 force = (cometTarget - rb.position).normalized * speed;
        rb.AddForce(force, ForceMode.Impulse);

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (rb.position.z > Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        Instantiate(explosion[Random.Range(0, explosion.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
