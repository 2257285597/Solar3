using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("游戏设置")]
    public GameObject celestialBodyPrefab; // 天体预制体
    public GameObject playerPrefab; // 玩家预制体
    public int initialAsteroidCount = 30; // 初始小行星数量
    
    [Header("生成范围")]
    public float spawnRangeMin = 10f;
    public float spawnRangeMax = 50f;
    
    private PlayerController player;
    private List<CelestialBody> allBodies = new List<CelestialBody>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    private void InitializeGame()
    {
        // 生成玩家
        SpawnPlayer();
        
        // 生成初始小行星场
        SpawnInitialAsteroids();
    }
    
    /// <summary>
    /// 生成玩家
    /// </summary>
    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("未设置玩家预制体！");
        }
    }
    
    /// <summary>
    /// 生成初始小行星
    /// </summary>
    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < initialAsteroidCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle.normalized * Random.Range(spawnRangeMin, spawnRangeMax);
            float randomMass = Random.Range(0.5f, 3f);
            
            SpawnAsteroid(randomPos, randomMass);
        }
    }
    
    /// <summary>
    /// 生成小行星
    /// </summary>
    public void SpawnAsteroid(Vector2 position, float mass)
    {
        if (celestialBodyPrefab != null)
        {
            Vector3 pos3D = new Vector3(position.x, position.y, 0);
            GameObject asteroid = Instantiate(celestialBodyPrefab, pos3D, Quaternion.identity);
            CelestialBody body = asteroid.GetComponent<CelestialBody>();
            
            if (body != null)
            {
                body.mass = mass;
                body.currentStage = EvolutionStage.Meteorite;
                body.UpdatePhysicalProperties();
                allBodies.Add(body);
            }
        }
    }
    
    /// <summary>
    /// 显示行星分支选择界面
    /// </summary>
    public void ShowPlanetBranchSelection(CelestialBody body)
    {
        UIManager.Instance?.ShowBranchSelection(body);
    }
    
    /// <summary>
    /// 显示突变选择界面
    /// </summary>
    public void ShowMutationSelection(CelestialBody body)
    {
        List<Mutation> mutations = MutationDatabase.Instance?.GetRandomMutations(3, body);
        UIManager.Instance?.ShowMutationSelection(body, mutations);
    }
    
    /// <summary>
    /// 持续生成小行星（防止场景空荡）
    /// </summary>
    private void Update()
    {
        // 每隔一段时间检查并补充小行星
        if (allBodies.Count < 20)
        {
            Vector2 randomPos = Random.insideUnitCircle.normalized * Random.Range(spawnRangeMin, spawnRangeMax);
            SpawnAsteroid(randomPos, Random.Range(0.5f, 2f));
        }
    }
}
