using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public Transform star;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SpawnComet());
    }

    IEnumerator SpawnComet()
    {
            Instantiate(star);
            yield return new WaitForSeconds(Random.Range(0, 3));
    }
}
