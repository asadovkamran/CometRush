using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public Transform comet;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnComet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnComet()
    {
        while (true)
        {
            Instantiate(comet);
            yield return new WaitForSeconds(Random.Range(0, 3));
        }

    }
}
