using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小行星生态管理器 - 负责生成、回收、密度保障
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [Header("生成范围")]
    public float spawnRangeMin = 25f;
    public float spawnRangeMax = 80f;

    [Header("总量与频率")]
    public int targetAsteroidCount = 35;
    public float spawnCheckInterval = 0.5f;
    public int maxSpawnPerTick = 5;

    [Header("玩家周边资源保障")]
    public float nearbyCheckRadius = 70f;
    public int minNearbyAsteroids = 12;

    [Header("前方优先生成")]
    [Range(0f, 1f)]
    public float forwardSpawnBias = 0.7f;
    public float minVelocityForForwardBias = 1f;
    public float forwardConeHalfAngle = 70f;

    [Header("远距回收")]
    public float recycleDistance = 220f;

    [Header("质量范围")]
    public float minMass = 0.5f;
    public float maxMass = 2.2f;

    private GameObject asteroidPrefab;
    private Transform playerTransform;
    private Rigidbody playerRb;
    private readonly List<CelestialBody> activeBodies = new List<CelestialBody>();
    private float spawnTimer;
    private bool initialized;

    public void Initialize(PlayerController player, GameObject prefab, int initialCount)
    {
        if (player == null)
        {
            Debug.LogError("AsteroidSpawner 初始化失败：玩家为空");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("AsteroidSpawner 初始化失败：小行星预制体为空");
            return;
        }

        asteroidPrefab = prefab;
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody>();
        spawnTimer = 0f;
        initialized = true;

        SpawnInitialAsteroids(initialCount);
    }

    private void Update()
    {
        if (!initialized || playerTransform == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnCheckInterval)
        {
            return;
        }

        spawnTimer = 0f;
        MaintainPopulation();
    }

    private void SpawnInitialAsteroids(int initialCount)
    {
        int count = Mathf.Max(0, initialCount);
        for (int i = 0; i < count; i++)
        {
            SpawnAsteroid(GetSpawnPositionAroundPlayer(false), Random.Range(minMass, maxMass));
        }
    }

    private void SpawnAsteroid(Vector2 position, float mass)
    {
        if (asteroidPrefab == null) return;

        GameObject asteroid = Instantiate(asteroidPrefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        CelestialBody body = asteroid.GetComponent<CelestialBody>();
        if (body == null)
        {
            Destroy(asteroid);
            return;
        }

        body.mass = mass;
        body.currentStage = EvolutionStage.Meteorite;
        body.UpdatePhysicalProperties();

        Rigidbody bodyRb = asteroid.GetComponent<Rigidbody>();
        if (bodyRb != null)
        {
            Vector2 randomVel = Random.insideUnitCircle * Random.Range(0.2f, 1.6f);
            bodyRb.velocity = new Vector3(randomVel.x, randomVel.y, 0f);
        }

        activeBodies.Add(body);
    }

    private Vector2 GetSpawnPositionAroundPlayer(bool preferForward)
    {
        Vector2 center = playerTransform != null ? (Vector2)playerTransform.position : Vector2.zero;
        float distance = Random.Range(spawnRangeMin, spawnRangeMax);

        if (preferForward && playerRb != null && playerRb.velocity.magnitude >= minVelocityForForwardBias && Random.value <= forwardSpawnBias)
        {
            Vector2 forward = playerRb.velocity.normalized;
            float randomAngle = Random.Range(-forwardConeHalfAngle, forwardConeHalfAngle);
            float rad = randomAngle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(
                forward.x * Mathf.Cos(rad) - forward.y * Mathf.Sin(rad),
                forward.x * Mathf.Sin(rad) + forward.y * Mathf.Cos(rad)
            );

            return center + dir.normalized * distance;
        }

        Vector2 randomDir = Random.insideUnitCircle;
        if (randomDir.sqrMagnitude < 0.001f)
        {
            randomDir = Vector2.right;
        }

        return center + randomDir.normalized * distance;
    }

    private void MaintainPopulation()
    {
        CleanupDeadBodies();
        RecycleFarAsteroids();

        int activeCount = activeBodies.Count;
        int spawnedThisTick = 0;

        if (activeCount < targetAsteroidCount)
        {
            int needByTotal = Mathf.Min(maxSpawnPerTick, targetAsteroidCount - activeCount);
            for (int i = 0; i < needByTotal; i++)
            {
                SpawnAsteroid(GetSpawnPositionAroundPlayer(true), Random.Range(minMass, maxMass));
                spawnedThisTick++;
            }
        }

        int nearbyCount = CountNearbyAsteroids(nearbyCheckRadius);
        if (nearbyCount < minNearbyAsteroids && spawnedThisTick < maxSpawnPerTick)
        {
            int needNearby = Mathf.Min(maxSpawnPerTick - spawnedThisTick, minNearbyAsteroids - nearbyCount);
            for (int i = 0; i < needNearby; i++)
            {
                SpawnAsteroid(GetSpawnPositionAroundPlayer(true), Random.Range(minMass, maxMass));
            }
        }
    }

    private void CleanupDeadBodies()
    {
        activeBodies.RemoveAll(body => body == null);
    }

    private int CountNearbyAsteroids(float radius)
    {
        if (playerTransform == null) return 0;

        Vector2 center = playerTransform.position;
        float radiusSqr = radius * radius;
        int count = 0;

        for (int i = 0; i < activeBodies.Count; i++)
        {
            CelestialBody body = activeBodies[i];
            if (body == null) continue;

            Vector2 delta = (Vector2)body.transform.position - center;
            if (delta.sqrMagnitude <= radiusSqr)
            {
                count++;
            }
        }

        return count;
    }

    private void RecycleFarAsteroids()
    {
        if (playerTransform == null) return;

        Vector2 center = playerTransform.position;
        float recycleDistanceSqr = recycleDistance * recycleDistance;

        for (int i = 0; i < activeBodies.Count; i++)
        {
            CelestialBody body = activeBodies[i];
            if (body == null) continue;

            Vector2 delta = (Vector2)body.transform.position - center;
            if (delta.sqrMagnitude > recycleDistanceSqr)
            {
                Vector2 newPos = GetSpawnPositionAroundPlayer(false);
                body.transform.position = new Vector3(newPos.x, newPos.y, 0f);

                Rigidbody bodyRb = body.GetComponent<Rigidbody>();
                if (bodyRb != null)
                {
                    bodyRb.velocity = Vector3.zero;
                    bodyRb.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
