using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 突变数据 - Roguelike 强化系统
/// 注意：这是纯数据类，不继承 MonoBehaviour，不能添加到 GameObject 上
/// </summary>
[System.Serializable]
public class Mutation
{
    public string mutationName;
    public string description;
    public MutationType type;
    public Sprite icon;

    // 效果数值
    public float massBonus = 0f;
    public float gravityBonus = 0f;
    public float speedBonus = 0f;
    public float defenseBonus = 0f;

    // 特殊效果
    public bool grantsTeleport = false;
    public bool grantsLifeSteal = false;
    public bool grantsDysonSphere = false;

    public void ApplyTo(CelestialBody body)
    {
        if (massBonus != 0)
        {
            body.mass *= (1 + massBonus);
        }

        if (gravityBonus != 0)
        {
            body.gravityStrength *= (1 + gravityBonus);
        }

        // TODO: 应用其他效果

        Debug.Log($"应用突变: {mutationName}");
    }
}

public enum MutationType
{
    Physical,       // 物理类
    Ecology,        // 生态类
    Technology,     // 科技类
    BranchSpecific  // 分支专属
}
