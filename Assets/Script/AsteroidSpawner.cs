using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小行星生成器 - 动态生成环境天体
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject asteroidPrefab;
    public int maxAsteroids = 50;
    public float spawnInterval = 2f;
    public float spawnDistance = 30f;
    
    [Header("质量范围")]
    public float minMass = 0.3f;
    public float maxMass = 5f;
    
    private Transform playerTransform;
    private List<GameObject> activeAsteroids = new List<GameObject>();
    private float lastSpawnTime;
    
    private void Start()
    {
        // 查找玩家
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        lastSpawnTime = Time.time;
    }
    
    private void Update()
    {
        // 清理已销毁的引用
        activeAsteroids.RemoveAll(a => a == null);
        
        // 如果数量不足，生成新的
        if (activeAsteroids.Count < maxAsteroids && Time.time - lastSpawnTime > spawnInterval)
        {
            SpawnRandomAsteroid();
            lastSpawnTime = Time.time;
        }
    }
    
    private void SpawnRandomAsteroid()
    {
        if (asteroidPrefab == null) return;
        
        // 在玩家周围的环形区域生成
        Vector3 spawnPos;
        if (playerTransform != null)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            spawnPos = playerTransform.position + new Vector3(randomDir.x, randomDir.y, 0) * spawnDistance;
        }
        else
        {
            Vector2 random2D = Random.insideUnitCircle * spawnDistance;
            spawnPos = new Vector3(random2D.x, random2D.y, 0);
        }
        
        // 生成小行星
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
        CelestialBody body = asteroid.GetComponent<CelestialBody>();
        
        if (body != null)
        {
            body.mass = Random.Range(minMass, maxMass);
            body.currentStage = EvolutionStage.Meteorite;
            
            // 给一个随机初速度
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector2 random2DVel = Random.insideUnitCircle * Random.Range(0.5f, 2f);
                rb.velocity = new Vector3(random2DVel.x, random2DVel.y, 0);
            }
        }
        
        activeAsteroids.Add(asteroid);
    }
}
