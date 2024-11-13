using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "Game/Constants")]
public class GameConstants : ScriptableObject
{
    public float MIN_SPAWN_DISTANCE = 20f;
    public float MAX_SPAWN_DISTANCE = 100f;

    public float COMET_SPAWN_HORIZONTAL_PADDING_PERCENT = 10;
    public float COMET_SPAWN_VERTICAL_PADDING_PERCENT = 10;

    public float COMET_COLLIDER_RADIUS_SCALE = 1.5f;

    /** Where the spawned comets will aim for in relation to camera position */
    public float MIN_TARGET_DEPTH = 1f;
    public float MAX_TARGET_DEPTH = 5f;
    public int MAX_SIMULTANEOUS_COMETS = 5;

    public float COMET_SPAWN_INTERVAL_MIN_SECONDS = 0f;
    public float COMET_SPAWN_INTERVAL_MAX_SECONDS = 3f;

    public float COMET_BASE_SPEED = 10f;
    public float COMET_SPEED_VARIANCE = 5f;

    public float STAR_SPAWN_INTERVAL_MIN_SECONDS = 0f;
    public float STAR_SPAWN_INTERVAL_MAX_SECONDS = 3f;

    public float STAR_MIN_SPAWN_DISTANCE = 20f;
    public float STAR_MAX_SPAWN_DISTANCE = 100f;

    public float PLAYER_MAX_HEALTH = 100f;
    public float PLAYER_SHIELDS_CAPACITY = 100f;
    public float SHIELD_REGEN_RATE = 0.1f;

    public float DIFFICULTY_COEFFICIENT = 0.005f;

    public float HEALTH_DROP_PROBABILITY = 10f; // 0 - 100%
    public float HEALTH_ITEM_HEAL_AMOUNT = 20f;
}