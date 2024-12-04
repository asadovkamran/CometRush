using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    private float _time = 0;

    public float Timer;

    private void Start()
    {
        Destroy(gameObject, Timer);
    }
}
