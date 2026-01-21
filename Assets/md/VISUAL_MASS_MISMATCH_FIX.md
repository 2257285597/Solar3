# 视觉与质量不匹配问题诊断

## ?? 问题描述

**用户报告**：一个天体视觉上比我小，但质量却比我大

## ?? 问题原因分析

### 原因 1: baseRadius 不一致 ? 最可能

所有天体的体积由以下公式计算：
```csharp
半径 = baseRadius × mass^0.33
体积 = 半径 × 2  // 直径
```

如果不同天体的 `baseRadius` 不同，就会出现视觉和质量不匹配：

**示例：**
```
天体 A: mass=10, baseRadius=0.5 → 视觉半径 = 0.5 × 10^0.33 × 2 ≈ 2.15
天体 B: mass=5,  baseRadius=1.0 → 视觉半径 = 1.0 × 5^0.33  × 2 ≈ 3.42
```
**结果**：质量 10 的看起来比质量 5 的小！?

---

### 原因 2: 手动修改了 Scale

如果在 Unity Inspector 中手动修改了某个物体的 `Transform.Scale`，而没有相应修改 `mass`，会导致不一致。

---

### 原因 3: 预制体配置不同

玩家预制体和小行星预制体可能有不同的 `baseRadius` 设置。

---

## ? 解决方案

### 方案 1: 统一所有 baseRadius（推荐）

**步骤：**

1. **检查玩家预制体**
   - Project 窗口 → 选中 `Player` 预制体
   - Inspector → `Player Controller` → `Base Radius`
   - 确保设置为 **0.5**

2. **检查小行星预制体**
   - Project 窗口 → 选中 `Asteroid` 预制体
   - Inspector → `Celestial Body` → `Base Radius`
   - 确保设置为 **0.5**

3. **检查场景中的实例**
   - 如果场景中有手动放置的天体
   - 检查它们的 `Base Radius` 也是 **0.5**

---

### 方案 2: 使用调试工具

**新增功能**：现在可以在游戏中实时显示质量！

1. **启用质量标签**
   - 选中任意天体
   - Inspector → `Celestial Body`
   - 勾选 `Show Mass Label` ?

2. **运行游戏**
   - 每个天体头顶会显示 `M:质量值`
   - 颜色与天体颜色相同
   - 一眼就能看出质量大小

**效果预览：**
```
    M:10.5
      ●      ← 玩家（质量 10.5）
    
  M:2.3
    ●        ← 小行星（质量 2.3）
```

---

### 方案 3: 使用控制台警告

代码已添加自动检测，如果 `baseRadius` 不是标准值 0.5，会在 Console 输出警告：

```
?? Player 的 baseRadius (1.0) 与标准值 (0.5) 不同，可能导致视觉不一致！
```

**查看方式：**
- 运行游戏后打开 Console 窗口（Ctrl+Shift+C）
- 查找黄色警告信息

---

## ?? 诊断步骤

### 第 1 步：检查所有预制体的 baseRadius

```
? 检查清单：
□ Player 预制体 → Base Radius = 0.5
□ Asteroid 预制体 → Base Radius = 0.5
□ 所有手动创建的天体 → Base Radius = 0.5
```

### 第 2 步：启用质量显示

1. 选中 `Player` 预制体
2. 勾选 `Show Mass Label`
3. 运行游戏
4. 观察每个天体显示的质量值

### 第 3 步：比对视觉和质量

运行游戏并记录：

| 天体名称 | 显示质量 | 视觉大小（目测）| 是否匹配 |
|----------|----------|-----------------|----------|
| Player   | 10.0     | 中等            | ？       |
| Asteroid1| 2.5      | 小              | ?        |
| Asteroid2| 15.0     | 大              | ?        |

---

## ?? 质量与体积对照表（baseRadius = 0.5）

| 质量 | 视觉半径 | 视觉直径 | 相对大小 |
|------|----------|----------|----------|
| 1    | 0.5      | 1.0      | 基准 (100%) |
| 2    | 0.63     | 1.26     | 126% |
| 5    | 0.85     | 1.71     | 171% |
| 10   | 1.08     | 2.15     | 215% |
| 20   | 1.36     | 2.71     | 271% |
| 50   | 1.84     | 3.68     | 368% |
| 100  | 2.32     | 4.64     | 464% |

**规律**：质量翻倍，体积增加约 26%（因为使用立方根）

---

## ?? 快速修复脚本

如果你想一键统一所有天体的 baseRadius，可以在场景中创建一个临时脚本：

```csharp
// FixBaseRadius.cs
using UnityEngine;

public class FixBaseRadius : MonoBehaviour
{
    [ContextMenu("修复所有天体的 baseRadius")]
    void FixAllBaseRadius()
    {
        CelestialBody[] allBodies = FindObjectsOfType<CelestialBody>();
        
        foreach (var body in allBodies)
        {
            body.baseRadius = 0.5f;
            body.UpdatePhysicalProperties();
            Debug.Log($"已修复 {body.gameObject.name} 的 baseRadius");
        }
        
        Debug.Log($"? 已修复 {allBodies.Length} 个天体");
    }
}
```

**使用方法：**
1. 创建空物体，添加此脚本
2. 右键点击脚本组件 → "修复所有天体的 baseRadius"
3. 查看 Console 确认修复结果

---

## ?? 最佳实践

### 1. 预制体配置标准

所有天体预制体应使用统一配置：
```
Base Radius: 0.5
Mass: 由代码动态设置
Scale: (1, 1, 1) 不要手动修改
```

### 2. 只通过 mass 控制大小

? **正确做法：**
```csharp
body.mass = 10f;  // 通过代码设置质量
body.UpdatePhysicalProperties();  // 自动计算体积
```

? **错误做法：**
```csharp
transform.localScale = Vector3.one * 5;  // 不要手动设置 Scale！
```

### 3. 使用调试工具

在开发阶段：
- 勾选 `Show Mass Label` 查看实时质量
- 开启 Gizmos 查看引力范围
- 查看 Console 警告信息

---

## ?? 测试方案

### 测试 1: 视觉一致性测试

1. 创建 3 个小行星：质量分别为 1, 5, 10
2. 观察大小关系：应该是 1 < 5 < 10
3. 启用 `Show Mass Label` 确认数值

### 测试 2: 吞噬测试

```
玩家质量 10，能吞噬：
  - 质量 1~8 的小行星 ?
  
玩家质量 10，不能吞噬：
  - 质量 8~12.5 的小行星（弹开）
  
玩家质量 10，会被吞噬：
  - 质量 > 12.5 的小行星 ?
```

---

## ? 常见问题

### Q: 为什么要用立方根（^0.33）？

**A**: 模拟真实物理：
- 质量 ∝ 体积 ∝ 半径?
- 所以 半径 ∝ 质量^(1/3)

这样质量增长不会让体积增长过快。

### Q: 可以改成平方根吗？

**A**: 可以，修改 `UpdatePhysicalProperties` 中的：
```csharp
float radius = baseRadius * Mathf.Pow(mass, 0.5f);  // 改为 0.5
```
**效果**：体积增长更快，视觉冲击更强。

### Q: 不同阶段可以有不同 baseRadius 吗？

**A**: 可以，但不推荐。如果要这样做：
```csharp
protected override void TriggerEvolution()
{
    base.TriggerEvolution();
    
    // 根据阶段调整 baseRadius
    switch (currentStage)
    {
        case EvolutionStage.Star:
            baseRadius = 0.7f;  // 恒星更大
            break;
        case EvolutionStage.BlackHole:
            baseRadius = 0.3f;  // 黑洞更小更密
            break;
    }
    
    UpdatePhysicalProperties();
}
```

---

## ? 检查清单

解决"视觉与质量不匹配"问题：

- [ ] 所有预制体的 `Base Radius = 0.5`
- [ ] 没有手动修改任何天体的 `Transform.Scale`
- [ ] 启用 `Show Mass Label` 确认质量显示正确
- [ ] Console 没有 baseRadius 警告
- [ ] 进行吞噬测试：大吃小正常工作
- [ ] 视觉大小与质量成正比

全部打勾后，问题应该已解决！?

---

现在运行游戏，勾选 `Show Mass Label`，就能直观看到每个天体的真实质量了！
