using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 突变数据库 - 管理所有可用的突变选项
/// 这个类继承 MonoBehaviour，可以添加到 GameObject 上
/// </summary>
public class MutationDatabase : MonoBehaviour
{
    public static MutationDatabase Instance { get; private set; }
    
    [Header("突变配置")]
    public List<Mutation> allMutations = new List<Mutation>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeMutations();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeMutations()
    {
        // 物理类突变
        allMutations.Add(new Mutation
        {
            mutationName = "致密内核",
            description = "质量+20%，密度增加",
            type = MutationType.Physical,
            massBonus = 0.2f
        });
        
        allMutations.Add(new Mutation
        {
            mutationName = "引力弹弓",
            description = "引力强度+50%",
            type = MutationType.Physical,
            gravityBonus = 0.5f
        });
        
        // 生态类突变
        allMutations.Add(new Mutation
        {
            mutationName = "星际孢子",
            description = "碰撞时持续偷取质量",
            type = MutationType.Ecology,
            grantsLifeSteal = true
        });
        
        // 科技类突变
        allMutations.Add(new Mutation
        {
            mutationName = "反物质引擎",
            description = "获得短距瞬移能力",
            type = MutationType.Technology,
            grantsTeleport = true
        });
        
        allMutations.Add(new Mutation
        {
            mutationName = "戴森球框架",
            description = "恒星阶段：能量转化为护盾",
            type = MutationType.Technology,
            grantsDysonSphere = true
        });
        
        // 分支专属突变
        allMutations.Add(new Mutation
        {
            mutationName = "永冻土",
            description = "冰封系：减速范围扩大一倍",
            type = MutationType.BranchSpecific
        });
        
        allMutations.Add(new Mutation
        {
            mutationName = "行军蚁",
            description = "战争系：撞击伤害随速度指数提升",
            type = MutationType.BranchSpecific
        });
    }
    
    /// <summary>
    /// 获取随机突变选项
    /// </summary>
    public List<Mutation> GetRandomMutations(int count, CelestialBody body)
    {
        List<Mutation> available = new List<Mutation>(allMutations);
        List<Mutation> selected = new List<Mutation>();
        
        // 过滤掉不适用的突变
        available.RemoveAll(m => 
            m.type == MutationType.BranchSpecific && 
            body.planetBranch == PlanetBranch.None
        );
        
        // 随机选择
        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, available.Count);
            selected.Add(available[randomIndex]);
            available.RemoveAt(randomIndex);
        }
        
        return selected;
    }
}
