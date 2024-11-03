using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDG;
using System;

public class HitDetection : MonoBehaviour
{
    public LayerMask layer;
   

    public static event Action<GameObject> OnCometHit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                OnCometHit?.Invoke(hit.transform.gameObject);
                // vibrate android device
                Vibration.Vibrate(25, 255);
            }
        }
    }
}
