using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机跟随脚本 - 支持 2D 和 3D 项目（优化防抖动版本）
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    [Header("跟随设置")]
    [Tooltip("位置跟随速度")]
    public float smoothSpeed = 8f;
    [Tooltip("使用物理插值（推荐，防止抖动）")]
    public bool usePhysicsInterpolation = true;
    
    [Header("基础距离设置")]
    [Tooltip("基础 Z 偏移（负数）")]
    public float baseZOffset = -20f;
    
    [Header("动态缩放")]
    [Tooltip("启用根据玩家质量/体积动态调整距离")]
    public bool dynamicZoom = true;
    [Tooltip("最小距离（离玩家最近）")]
    public float minDistance = 10f;
    [Tooltip("最大距离（离玩家最远）")]
    public float maxDistance = 100f;
    [Tooltip("距离调整速度")]
    public float zoomSpeed = 2f;
    [Tooltip("距离缩放倍率（越大相机退得越远）")]
    public float distanceMultiplier = 2.5f;
    
    private Camera cam;
    private float currentDistance;
    private float targetDistance;
    private bool isPerspective;
    
    // 防抖动缓存
    private Vector3 velocity = Vector3.zero;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
        
        // 检测相机类型
        isPerspective = !cam.orthographic;
        
        // 初始化目标距离
        currentDistance = Mathf.Abs(baseZOffset);
        targetDistance = currentDistance;
        
        // 如果目标已设置，立即对齐位置（避免初始跳跃）
        if (target != null)
        {
            Vector3 offset = new Vector3(0, 0, -currentDistance);
            transform.position = target.position + offset;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        // 动态缩放：根据玩家质量调整距离
        if (dynamicZoom)
        {
            CelestialBody body = target.GetComponent<CelestialBody>();
            if (body != null)
            {
                UpdateCameraDistance(body);
            }
        }
        
        // 平滑距离过渡
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, zoomSpeed * Time.deltaTime);
        
        // 计算目标位置
        Vector3 offset = new Vector3(0, 0, isPerspective ? -currentDistance : baseZOffset);
        Vector3 desiredPosition = target.position + offset;
        
        // 使用 SmoothDamp 替代 Lerp，提供更自然的缓动（防抖动关键）
        if (usePhysicsInterpolation)
        {
            // SmoothDamp 提供物理感的平滑，不会产生抖动
            float smoothTime = 1f / smoothSpeed;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
        else
        {
            // 传统 Lerp 方法
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// 根据天体质量更新相机距离
    /// </summary>
    private void UpdateCameraDistance(CelestialBody body)
    {
        if (isPerspective)
        {
            // 3D 透视相机：调整 Z 距离
            // 公式：距离 = 体积半径 × 倍率 + 固定偏移
            float bodyRadius = body.transform.localScale.x / 2f; // 天体半径
            
            // 改进公式：确保小天体和大天体都有合适的视野
            float baseDistance = 15f; // 基础距离
            float scaledDistance = bodyRadius * distanceMultiplier;
            float desiredDistance = baseDistance + scaledDistance;
            
            // 限制最大最小距离
            targetDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        }
        else
        {
            // 2D 正交相机：调整 Orthographic Size
            float bodyRadius = body.transform.localScale.x / 2f;
            float desiredSize = 5f + bodyRadius * 1.5f;
            targetDistance = Mathf.Clamp(desiredSize, minDistance, maxDistance);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetDistance, zoomSpeed * Time.deltaTime);
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        
        // 立即对齐到新目标（避免跳跃）
        if (newTarget != null)
        {
            Vector3 offset = new Vector3(0, 0, -currentDistance);
            transform.position = newTarget.position + offset;
            velocity = Vector3.zero; // 重置速度
        }
    }
}

