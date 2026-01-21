# 天体质量范围配置指南

## ?? 配置位置

### 1. 玩家进化阈值（Player Prefab）

在 **PlayerController** 组件中（已从 CelestialBody 继承）：

```
┌─────────────────────────────────────┐
│ Player Controller (Script)          │
├─────────────────────────────────────┤
│ ▼ 进化阈值配置                      │
│   Evolution To Small Planet: 10     │
│   Evolution To Planet: 50           │
│   Evolution To Star: 200            │
│   Evolution To Advanced: 500        │
│   Evolution To Black Hole: 1000     │
└─────────────────────────────────────┘
```

**修改这些值可以调整进化速度：**
- 减小数值 → 更快进化（简单模式）
- 增大数值 → 更慢进化（困难模式）

---

### 2. NPC 小行星质量范围（AsteroidSpawner）

在 **AsteroidSpawner** 组件中：

```
┌─────────────────────────────────────┐
│ Asteroid Spawner (Script)           │
├─────────────────────────────────────┤
│ ▼ 质量范围                          │
│   Min Mass: 0.3                     │
│   Max Mass: 5                       │
└─────────────────────────────────────┘
```

**修改这些值可以调整游戏难度：**
- 增加 Min/Max Mass → 小行星更"肥"，升级更快
- 减小 Min/Max Mass → 小行星更"瘦"，升级更慢

---

## ?? 默认质量范围表

| 阶段 | 中文名 | 质量范围 | 特征 |
|------|--------|----------|------|
| Meteorite | 陨石 | 1 - 10 | 起始阶段，体积最小 |
| SmallPlanet | 小行星 | 10 - 50 | 开始有引力 |
| Planet | 行星 | 50 - 200 | 选择分支（冰封/生命/战争）|
| Star | 恒星 | 200 - 500 | 发光，引力强 |
| RedGiant/Neutron/Pulsar | 高级恒星 | 500 - 1000 | 特殊能力 |
| BlackHole | 黑洞 | 1000+ | 终极形态 |

---

## ?? 推荐配置方案

### 配置 A：快速成长（街机模式）
```
进化阈值（在 Player Prefab 中）：
  Evolution To Small Planet: 5      （默认 10）
  Evolution To Planet: 20            （默认 50）
  Evolution To Star: 80              （默认 200）
  Evolution To Advanced: 200         （默认 500）
  Evolution To Black Hole: 500       （默认 1000）

小行星质量（在 AsteroidSpawner 中）：
  Min Mass: 0.5
  Max Mass: 8
```

**效果**：快速体验所有进化阶段

---

### 配置 B：默认平衡
```
进化阈值：
  Evolution To Small Planet: 10
  Evolution To Planet: 50
  Evolution To Star: 200
  Evolution To Advanced: 500
  Evolution To Black Hole: 1000

小行星质量：
  Min Mass: 0.3
  Max Mass: 5
```

**效果**：平衡的游戏节奏

---

### 配置 C：硬核生存
```
进化阈值：
  Evolution To Small Planet: 20
  Evolution To Planet: 100
  Evolution To Star: 500
  Evolution To Advanced: 1500
  Evolution To Black Hole: 3000

小行星质量：
  Min Mass: 0.1
  Max Mass: 2
```

**效果**：漫长的成长过程，更有成就感

---

### 配置 D：巨兽模式（快速到黑洞）
```
进化阈值：
  Evolution To Small Planet: 3
  Evolution To Planet: 10
  Evolution To Star: 30
  Evolution To Advanced: 80
  Evolution To Black Hole: 150

小行星质量：
  Min Mass: 1
  Max Mass: 15
```

**效果**：极快进化，体验黑洞阶段

---

## ?? 调整步骤

### 修改玩家进化速度：

1. 在 **Project 窗口**中找到 `Player` 预制体
2. 选中后在 Inspector 中找到 `Player Controller` 组件
3. 展开 **▼ 进化阈值配置**
4. 修改数值
5. **重要**：保存预制体（Ctrl+S 或 File > Save）

### 修改小行星质量：

1. 在 **Hierarchy** 中选中 `AsteroidSpawner` 对象
2. 在 Inspector 中找到 `Asteroid Spawner` 组件
3. 展开 **▼ 质量范围**
4. 修改 `Min Mass` 和 `Max Mass`
5. 无需保存，运行时生效

---

## ?? 质量与体积的关系

游戏中体积根据质量计算：

```csharp
半径 = 基础半径 × mass^0.33
```

**实际效果：**

| 质量 | 半径倍数 | 体积倍数 |
|------|----------|----------|
| 1 | 1.0× | 1× |
| 8 | 2.0× | 8× |
| 27 | 3.0× | 27× |
| 100 | 4.6× | 100× |
| 1000 | 10× | 1000× |

**所以**：质量增加 1000 倍，体积才增加 1000 倍，视觉上只大了 10 倍。

---

## ?? 平衡建议

### 原则 1：进化阈值呈指数增长
```
推荐比例：每阶段 × 4 - 5
例如：10 → 50 → 200 → 1000
```

### 原则 2：小行星质量要合理
```
Max Mass 不应超过第一个进化阈值的 50%
例如：进化阈值 10，Max Mass 应 ≤ 5
```

### 原则 3：后期加速
如果想让后期更快：
```csharp
// 在 CelestialBody.cs 的 AddMass 中添加倍率
public override void AddMass(float amount)
{
    // 后期吞噬获得额外加成
    float bonus = 1f;
    if (currentStage >= EvolutionStage.Star)
        bonus = 1.5f; // 恒星阶段获得 50% 额外质量
    
    mass += amount * bonus;
    // ...
}
```

---

## ?? 测试建议

### 快速测试进化系统：
1. 使用配置 A（快速成长）
2. 观察每个阶段的视觉效果和玩法
3. 确认所有进化阶段都能触发

### 平衡测试：
1. 使用配置 B（默认）
2. 计时：从开始到行星阶段应该在 2-5 分钟
3. 确保游戏节奏不无聊也不太急促

### 极限测试：
1. 把进化阈值设为 1, 2, 3, 4, 5...
2. 快速体验所有阶段
3. 检查是否有 BUG

---

## ?? 高级技巧

### 为不同难度创建多个预制体：

```
Player_Easy    (快速进化)
Player_Normal  (默认)
Player_Hard    (慢进化)
```

在 GameManager 中切换：
```csharp
public GameObject[] playerPrefabsByDifficulty;
public int difficulty = 1; // 0=简单, 1=普通, 2=困难

void SpawnPlayer() {
    Instantiate(playerPrefabsByDifficulty[difficulty]);
}
```

---

## ?? 常见问题

### Q: 为什么修改了数值但没有生效？
**A**: 需要修改**预制体**而不是场景中的实例。或者删除场景中的旧实例，让 GameManager 重新生成。

### Q: 如何让黑洞阶段不再进化？
**A**: 已经设置为 `float.MaxValue`，不会再进化。

### Q: 能否给每个阶段设置质量上限？
**A**: 可以，在 `AddMass` 中添加：
```csharp
if (currentStage == EvolutionStage.Planet && mass > 500f)
    mass = 500f; // 行星阶段质量上限
```

---

现在你可以轻松在 Unity Inspector 中调整所有质量相关的参数了！??
