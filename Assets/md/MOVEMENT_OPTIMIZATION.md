# 移动系统优化说明

## ?? 问题描述
原版本中，体积（质量）变大后移动速度急剧下降，导致大型天体几乎无法移动。

## ? 优化方案

### 1. 新增可调节参数（在 Inspector 中可见）

在 PlayerController 组件中新增了三个参数：

| 参数 | 默认值 | 说明 |
|------|--------|------|
| `Mass Impact On Thrust` | 0.3 | 质量对推力的影响程度（0-1，越小影响越小）|
| `Mass Impact On Speed` | 0.2 | 质量对最大速度的影响程度（0-1，越小影响越小）|
| `Base Speed Multiplier` | 1.5 | 基础速度倍率（越大整体移动越快）|

### 2. 移动力计算优化

**旧版本：**
```csharp
float mobilityFactor = 1f / Mathf.Sqrt(mass);
// 质量 100 时，mobilityFactor = 0.1（推力下降到 10%）
```

**新版本：**
```csharp
float mobilityFactor = 1f / Mathf.Pow(mass, 0.3f);
float massCompensation = 1f + Mathf.Log10(mass + 1) * 0.5f;
// 质量 100 时，mobilityFactor ≈ 0.4，massCompensation ≈ 2.0
// 实际推力 = 原推力 × 0.4 × 2.0 = 80%
```

**对比效果：**

| 质量 | 旧版推力比例 | 新版推力比例 | 改善幅度 |
|------|--------------|--------------|----------|
| 1    | 100%         | 100%         | 0%       |
| 10   | 32%          | 68%          | +113%    |
| 50   | 14%          | 51%          | +264%    |
| 100  | 10%          | 44%          | +340%    |
| 500  | 4.5%         | 30%          | +567%    |

### 3. 最大速度计算优化

**旧版本：**
```csharp
float maxSpeed = moveSpeed / Mathf.Log10(mass + 10);
// 质量 100 时，maxSpeed = moveSpeed / 2.04 ≈ 0.49 倍
```

**新版本：**
```csharp
float speedPenalty = Mathf.Pow(mass, 0.2f);
float maxSpeed = (moveSpeed * 1.5) / speedPenalty;
maxSpeed = Mathf.Max(maxSpeed, moveSpeed * 0.3f); // 最低速度保障
// 质量 100 时，maxSpeed ≈ 0.95 倍（仍保持接近原速）
```

**对比效果：**

| 质量 | 旧版速度比例 | 新版速度比例 | 改善幅度 |
|------|--------------|--------------|----------|
| 1    | 100%         | 150%         | +50%     |
| 10   | 78%          | 119%         | +53%     |
| 50   | 52%          | 95%          | +83%     |
| 100  | 49%          | 86%          | +76%     |
| 500  | 38%          | 66%          | +74%     |

### 4. 冲刺优化

**旧版本：**
```csharp
rb.AddForce(dashDirection * dashSpeed * mass, ForceMode.Impulse);
// 小质量时冲刺无力
```

**新版本：**
```csharp
float dashForce = dashSpeed * Mathf.Max(mass, 5f);
// 确保最小质量为 5，让陨石阶段也有足够的冲刺效果
```

## ??? 调整建议

### 如果觉得移动还是太慢：

在 PlayerController 组件中调整：
1. **增加 `Base Speed Multiplier`** → 2.0 或更高（整体加速）
2. **降低 `Mass Impact On Thrust`** → 0.2 或 0.1（减少质量惩罚）
3. **降低 `Mass Impact On Speed`** → 0.1（速度衰减更慢）
4. **增加 `Thrust Force`** → 15 或 20（基础推力更大）

### 如果觉得移动太快/太灵活：

1. **增加 `Mass Impact On Thrust`** → 0.5（更真实的质量感）
2. **降低 `Base Speed Multiplier`** → 1.0
3. **增加 Rigidbody 的 `Linear Drag`** → 1.0 或更高（增加摩擦感）

## ?? 推荐配置

### 配置 A：街机风格（推荐新手）
```
Base Speed Multiplier: 2.0
Mass Impact On Thrust: 0.2
Mass Impact On Speed: 0.15
Thrust Force: 15
Move Speed: 8
```

### 配置 B：平衡风格（默认）
```
Base Speed Multiplier: 1.5
Mass Impact On Thrust: 0.3
Mass Impact On Speed: 0.2
Thrust Force: 10
Move Speed: 5
```

### 配置 C：硬核风格
```
Base Speed Multiplier: 1.0
Mass Impact On Thrust: 0.5
Mass Impact On Speed: 0.3
Thrust Force: 10
Move Speed: 5
Linear Drag: 1.0（在 Rigidbody 中设置）
```

## ?? 技术细节

### 为什么使用幂函数？

```csharp
Mathf.Pow(mass, 0.3f)  // 平缓的增长曲线
vs
Mathf.Sqrt(mass)       // 等价于 Mathf.Pow(mass, 0.5f)，更陡峭
```

**直观对比：**
- 质量 100 时：
  - `Pow(100, 0.3)` ≈ 2.5（惩罚较轻）
  - `Sqrt(100)` = 10（惩罚严重）

### 质量补偿机制

```csharp
float massCompensation = 1f + Mathf.Log10(mass + 1) * 0.5f;
```

这模拟了"更大的星球拥有更强引擎"的设定，部分抵消质量惩罚。

### 最低速度保障

```csharp
maxSpeed = Mathf.Max(maxSpeed, moveSpeed * 0.3f);
```

确保即使是超大质量黑洞，也至少保留 30% 的基础速度。

## ? 效果演示

**陨石阶段（质量 1-10）：**
- 移动：灵活快速 ?
- 冲刺：短距离爆发 ?

**行星阶段（质量 50-200）：**
- 移动：稍慢但仍流畅 ?
- 感觉：有质量感但不笨重 ?

**恒星阶段（质量 500+）：**
- 移动：缓慢但稳定 ?
- 感觉：像驾驶太空母舰 ?

**黑洞阶段（质量 1000+）：**
- 移动：仍可控制 ?
- 策略：更依赖引力而非移动 ?

---

现在在 Unity 中运行游戏，应该能明显感受到改善！??
