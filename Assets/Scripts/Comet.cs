using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CometRush.Enums;

public class Comet : MonoBehaviour
{
    public float speed;
    public CometType type;
    private Vector3 cometTarget;

    public GameConstants GAME_CONSTANTS;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        HandleSpawn();
        HandlePushTowardsPlayer();
    }

    void FixedUpdate()
    {
        if (rb.position.z > Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }
    }

    private void HandleSpawn()
    {
        float spawnDistance = Random.Range(GAME_CONSTANTS.MIN_SPAWN_DISTANCE, GAME_CONSTANTS.MAX_SPAWN_DISTANCE);

        int horizontalPaddingPx = (int)(Camera.main.pixelWidth * GAME_CONSTANTS.COMET_SPAWN_HORIZONTAL_PADDING_PERCENT / 100);
        int verticalPaddingPx = (int)(Camera.main.pixelHeight * GAME_CONSTANTS.COMET_SPAWN_VERTICAL_PADDING_PERCENT / 100);

        Vector3 spawnPositionScreenPoint = new Vector3(
            Random.Range(horizontalPaddingPx, Camera.main.pixelWidth - horizontalPaddingPx),
            Random.Range(verticalPaddingPx, Camera.main.pixelHeight - verticalPaddingPx),
            spawnDistance
        );
        Vector3 spawnPositionWorld = Camera.main.ScreenToWorldPoint(spawnPositionScreenPoint);
        rb.position = spawnPositionWorld;
    }

    private void HandlePushTowardsPlayer()
    {
        Vector3 randomTargetPosition = new Vector3(
            Random.Range(0, Camera.main.pixelWidth),
            Random.Range(0, Camera.main.pixelHeight),
            -Random.Range(1, 5)
        );
        cometTarget = Camera.main.ScreenToWorldPoint(randomTargetPosition);

        speed = GAME_CONSTANTS.COMET_BASE_SPEED + Random.Range(-GAME_CONSTANTS.COMET_SPEED_VARIANCE, GAME_CONSTANTS.COMET_SPEED_VARIANCE);

        Vector3 force = (cometTarget - rb.position).normalized * speed;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
