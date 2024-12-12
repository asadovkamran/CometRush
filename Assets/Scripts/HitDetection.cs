using UnityEngine;
using System;

public class HitDetection : MonoBehaviour
{
    public LayerMask Layer;

    public static event Action<GameObject> OnCometHit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
            {
                OnCometHit?.Invoke(hit.transform.gameObject);
            }
        }
    }
}
