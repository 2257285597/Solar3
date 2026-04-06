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
    public GameObject playerPrefab; // 玩家预制体
    public GameObject celestialBodyPrefab; // 小行星预制体
    public int initialAsteroidCount = 30; // 初始小行星数量

    [Header("系统引用")]
    public AsteroidSpawner asteroidSpawner;
    
    private PlayerController player;
    
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
        EnsureSpaceBackground();

        // 生成玩家
        SpawnPlayer();

        // 初始化小行星系统
        InitializeAsteroidSpawner();
    }

    private void EnsureSpaceBackground()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        if (mainCam.GetComponent<SpaceBackgroundController>() == null)
        {
            mainCam.gameObject.AddComponent<SpaceBackgroundController>();
        }
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

    private void InitializeAsteroidSpawner()
    {
        if (player == null)
        {
            Debug.LogError("玩家未初始化，无法启动 AsteroidSpawner");
            return;
        }

        if (celestialBodyPrefab == null)
        {
            Debug.LogError("未设置小行星预制体，无法启动 AsteroidSpawner");
            return;
        }

        if (asteroidSpawner == null)
        {
            asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        }

        if (asteroidSpawner == null)
        {
            asteroidSpawner = gameObject.AddComponent<AsteroidSpawner>();
        }

        asteroidSpawner.Initialize(player, celestialBodyPrefab, initialAsteroidCount);
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
    
}
