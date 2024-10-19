using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public LayerMask layer;
    public GameObject[] explosions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                Destroy(hit.transform.gameObject);
                Instantiate(explosions[Random.Range(0, explosions.Length)], hit.transform.position, Quaternion.identity);
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
}
