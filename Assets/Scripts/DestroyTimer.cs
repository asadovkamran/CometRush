using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    private float _time = 0;

    public float Timer;

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= Timer)
        {
            Destroy(gameObject);
        }
    }
}
