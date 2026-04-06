using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : CelestialBody
{
    [Header("控制设置")]
    public float moveSpeed = 5f;
    public float thrustForce = 10f;
    public float dashSpeed = 20f;
    public float dashCooldown = 2f;
    
    [Header("移动平衡")]
    [Tooltip("基础速度倍率（越大整体移动越快）")]
    public float baseSpeedMultiplier = 2.5f;  // 从 1.5 提高到 2.5
    [Tooltip("质量速度成长系数（越大时，质量越大速度越快）")]
    public float massSpeedGrowth = 0.8f;
    
    [Header("技能")]
    public bool canDash = true;
    public bool canSplit = false; // 陨石阶段的分裂技能
    public float strongGravityMultiplier = 3f;
    
    private float lastDashTime = -999f;
    private bool isStrongGravityActive = false;
    
    // 输入
    private Vector2 moveInput;
    
    protected override void Start()
    {
        base.Start();
        
        // 玩家默认开始为陨石
        currentStage = EvolutionStage.Meteorite;
        mass = 1f;
        massToNextEvolution = 10f;
        
        // 立即更新物理属性和视觉
        UpdatePhysicalProperties();
        UpdateVisuals();
        
        // 立即更新 UI 显示
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMassDisplay(mass, massToNextEvolution);
            UIManager.Instance.UpdateStageDisplay(currentStage);
        }
        
        // 摄像机跟随
        Camera.main.GetComponent<CameraFollow>()?.SetTarget(transform);
        
        Debug.Log($"玩家初始化完成：质量={mass}, 阶段={currentStage}");
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        HandleMovement();
    }
    
    private void Update()
    {
        // 获取输入
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        // 更新速度显示
        if (rb != null)
        {
            UIManager.Instance?.UpdateVelocityDisplay(rb.velocity.magnitude);
        }
        
        // 冲刺（陨石阶段）
        if (Input.GetKeyDown(KeyCode.Space) && currentStage == EvolutionStage.Meteorite)
        {
            TryDash();
        }
        
        // 强引力波（按住）
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ActivateStrongGravity();
        }
        else
        {
            DeactivateStrongGravity();
        }
        
        // 技能释放（根据分支不同）
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility();
        }
        
        // 切换操作提示（按 H 键）
        if (Input.GetKeyDown(KeyCode.H))
        {
            UIManager.Instance?.ToggleControlHints();
        }
    }
    
    /// <summary>
    /// 处理移动
    /// </summary>
    private void HandleMovement()
    {
        // 质量正向增益：质量越大，移动和上限速度都越快
        float massSpeedBonus = 1f + Mathf.Log10(mass + 1f) * massSpeedGrowth;

        // 阶段加成：进化越高移动越快
        float stageBonus = GetStageMovementBonus();

        if (moveInput.magnitude > 0.1f)
        {
            Vector3 moveAcceleration = new Vector3(moveInput.x, moveInput.y, 0) * thrustForce;

            // 使用 Acceleration 模式，避免 Rigidbody 质量变大后加速度被动下降
            rb.AddForce(moveAcceleration * baseSpeedMultiplier * massSpeedBonus * stageBonus, ForceMode.Acceleration);
        }

        float maxSpeed = moveSpeed * baseSpeedMultiplier * massSpeedBonus * stageBonus;
        
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    
    /// <summary>
    /// 获取阶段移动加成
    /// </summary>
    private float GetStageMovementBonus()
    {
        switch (currentStage)
        {
            case EvolutionStage.Meteorite:
                return 1.0f;  // 陨石：正常速度
            case EvolutionStage.SmallPlanet:
                return 1.1f;  // 小行星：+10% 速度
            case EvolutionStage.Planet:
                return 1.3f;  // 行星：+30% 速度（重要！）
            case EvolutionStage.Star:
                return 1.5f;  // 恒星：+50% 速度
            case EvolutionStage.RedGiant:
            case EvolutionStage.NeutronStar:
            case EvolutionStage.Pulsar:
                return 1.7f;  // 高级阶段：+70% 速度
            case EvolutionStage.BlackHole:
                return 2.0f;  // 黑洞：+100% 速度（超快）
            default:
                return 1.0f;
        }
    }
    
    /// <summary>
    /// 尝试冲刺
    /// </summary>
    private void TryDash()
    {
        if (!canDash) return;
        if (Time.time - lastDashTime < dashCooldown) return;
        
        Vector3 dashDirection = moveInput.magnitude > 0.1f 
            ? new Vector3(moveInput.x, moveInput.y, 0).normalized 
            : Vector3.right;
        
        // 冲刺力度随质量缩放，但添加基础值保证小质量时也有效
        float dashForce = dashSpeed * Mathf.Max(mass, 5f);
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        
        lastDashTime = Time.time;
        
        Debug.Log($"冲刺！（质量: {mass:F1}）");
    }
    
    /// <summary>
    /// 激活强引力波
    /// </summary>
    private void ActivateStrongGravity()
    {
        if (!isStrongGravityActive)
        {
            isStrongGravityActive = true;
            gravityStrength *= strongGravityMultiplier;
        }
    }
    
    /// <summary>
    /// 停用强引力波
    /// </summary>
    private void DeactivateStrongGravity()
    {
        if (isStrongGravityActive)
        {
            isStrongGravityActive = false;
            gravityStrength /= strongGravityMultiplier;
        }
    }
    
    /// <summary>
    /// 使用技能
    /// </summary>
    private void UseAbility()
    {
        switch (planetBranch)
        {
            case PlanetBranch.IceFortress:
                // 极寒光环 - 减速周围敌人
                UseIceAura();
                break;
            case PlanetBranch.LifeCradle:
                // 文明反击 - 发射导弹
                UseCivilizationCounterattack();
                break;
            case PlanetBranch.WarPlanet:
                // 轨道轰炸
                UseOrbitalBombardment();
                break;
        }
    }
    
    private void UseIceAura()
    {
        Debug.Log("释放极寒光环！");
        // TODO: 实现减速效果
    }
    
    private void UseCivilizationCounterattack()
    {
        Debug.Log("发射文明导弹！");
        // TODO: 生成导弹
    }
    
    private void UseOrbitalBombardment()
    {
        Debug.Log("轨道轰炸！");
        // TODO: 发射高能粒子流
    }
    
    public override void AddMass(float amount)
    {
        base.AddMass(amount);
        
        // UI 更新
        UIManager.Instance?.UpdateMassDisplay(mass, massToNextEvolution);
    }
    
    protected override void TriggerEvolution()
    {
        base.TriggerEvolution();
        
        // 更新 UI 显示
        UIManager.Instance?.UpdateStageDisplay(currentStage);
        
        // 播放进化特效
        Debug.Log($"玩家进化至: {currentStage}！");
    }
}
