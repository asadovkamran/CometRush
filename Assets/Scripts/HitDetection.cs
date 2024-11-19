using UnityEngine;
using RDG;
using System;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private GameStatsSO _gameStatsSO;
    public LayerMask Layer;

    public static event Action<GameObject> OnCometHit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
            {
                _gameStatsSO.IncrementScore();
                OnCometHit?.Invoke(hit.transform.gameObject);
                // vibrate android device
                Vibration.Vibrate(25, 255);
            }
        }
    }
}
