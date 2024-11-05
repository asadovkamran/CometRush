using UnityEngine;

public class Star : MonoBehaviour
{
    public float Speed;
    public GameConstants GAME_CONSTANTS;
    public ObjectPool Pool;

    private Rigidbody _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Pool = GameObject.Find("StarsPool").GetComponent<ObjectPool>();

        SetInitialPosition();
    }

    private void SetInitialPosition()
    {
        float spawnDistance = Random.Range(GAME_CONSTANTS.STAR_MIN_SPAWN_DISTANCE, GAME_CONSTANTS.STAR_MAX_SPAWN_DISTANCE);

        Vector3 randomSpawnPosition = new Vector3(Random.Range(0, Camera.main.pixelWidth),
            Random.Range(0, Camera.main.pixelHeight), spawnDistance);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(randomSpawnPosition);


        _rb.position = worldPosition;
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        SetInitialPosition();
    }

    private void FixedUpdate()
    {
        _rb.velocity = Vector3.forward * Speed * Time.fixedDeltaTime;
        _rb.AddForce(_rb.velocity, ForceMode.Force);

        if (_rb.position.z > Camera.main.transform.position.z)
        {
            Pool.ReturnToPool(gameObject);
        }
    }
}
