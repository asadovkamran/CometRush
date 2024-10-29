using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CometRush.Enums;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;

public class Comet : MonoBehaviour
{
    public float speed;
    public CometType type;
    public float cometDamage;
    private Vector3 cometTarget;
    public Mesh[] meshes;

    public GameConstants GAME_CONSTANTS;
    private Rigidbody rb;
    private MeshFilter filter;
    private SphereCollider sphereCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        filter = GetComponent<MeshFilter>();

        // add sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius *= GAME_CONSTANTS.COMET_COLLIDER_RADIUS_SCALE;

        Mesh currentMesh = meshes[UnityEngine.Random.Range(0, meshes.Length)];
        filter.mesh = currentMesh;


        HandleSpawn();
        HandlePushTowardsPlayer();
    }

    void FixedUpdate()
    {
        if (rb.position.z > Camera.main.transform.position.z)
        {
            GameManager.Instance.UpdateShields(-cometDamage);
            Destroy(gameObject);
            FullScreenDamage.Instance.RunHurt();
        }
    }

    private void HandleSpawn()
    {
        float spawnDistance = UnityEngine.Random.Range(GAME_CONSTANTS.MIN_SPAWN_DISTANCE, GAME_CONSTANTS.MAX_SPAWN_DISTANCE);

        int horizontalPaddingPx = (int)(Camera.main.pixelWidth * GAME_CONSTANTS.COMET_SPAWN_HORIZONTAL_PADDING_PERCENT / 100);
        int verticalPaddingPx = (int)(Camera.main.pixelHeight * GAME_CONSTANTS.COMET_SPAWN_VERTICAL_PADDING_PERCENT / 100);

        Vector3 spawnPositionScreenPoint = new Vector3(
            UnityEngine.Random.Range(horizontalPaddingPx, Camera.main.pixelWidth - horizontalPaddingPx),
            UnityEngine.Random.Range(verticalPaddingPx, Camera.main.pixelHeight - verticalPaddingPx),
            spawnDistance
        );
        Vector3 spawnPositionWorld = Camera.main.ScreenToWorldPoint(spawnPositionScreenPoint);
        rb.position = spawnPositionWorld;
    }

    private void HandlePushTowardsPlayer()
    {
        Vector3 randomTargetPosition = new Vector3(
            UnityEngine.Random.Range(0, Camera.main.pixelWidth),
            UnityEngine.Random.Range(0, Camera.main.pixelHeight),
            -UnityEngine.Random.Range(1, 5)
        );
        cometTarget = Camera.main.ScreenToWorldPoint(randomTargetPosition);

        speed = GAME_CONSTANTS.COMET_BASE_SPEED + UnityEngine.Random.Range(-GAME_CONSTANTS.COMET_SPEED_VARIANCE, GAME_CONSTANTS.COMET_SPEED_VARIANCE) + GameManager.Instance.getDifficulty();

        Vector3 force = (cometTarget - rb.position).normalized * speed;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
