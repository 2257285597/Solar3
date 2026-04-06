# Solar3 项目速查清单（低 Token 模式）

目标：减少无意义上下文读取，优先按需定位。

## 0) 模块速查入口（优先读）
- 玩法规则与流程：QUICK_MODULE_GAMEPLAY_LOOP.md
- 生成与回收系统：QUICK_MODULE_SPAWNING.md
- 玩家移动与技能入口：QUICK_MODULE_PLAYER_MOTION.md
- UI 与突变：QUICK_MODULE_UI_MUTATION.md
- 背景与相机：QUICK_MODULE_VISUAL_CAMERA.md

## 0.1) 自动低 Token Skill
- Workspace Skill：../../.github/skills/low-token-unity-workflow/SKILL.md
- 作用：按模块优先读取、局部编辑、局部校验，避免全量扫描。

## 1) 当前核心结构
- 游戏编排与入口：Assets/Script/GameManager.cs
- 玩家移动与技能入口：Assets/Script/PlayerController.cs
- 天体基础规则（吞噬/进化/分支）：Assets/Script/CelestialBody.cs
- 小行星生态系统（动态生成/回收/密度保障）：Assets/Script/AsteroidSpawner.cs
- UI 面板与分支/突变选择：Assets/Script/UIManager.cs
- 突变数据与应用逻辑：Assets/Script/Mutation.cs, Assets/Script/MutationDatabase.cs
- 相机跟随：Assets/Script/CameraFollow.cs
- 背景控制（当前为单层纯深蓝）：Assets/Script/SpaceBackgroundController.cs

## 2) 当前已落地状态（2026-04-06）
- 编码：主要脚本已统一为 UTF-8。
- 移动：质量越大，速度和加速上限越高；使用质量无关加速度模式。
- 分支与突变：流程链路已通（进化 -> 选择 UI -> 应用）。
- 技能：分支主动技能多为占位（日志 + TODO）。
- 卫星：仅有字段与上限加成，尚未形成可玩系统。
- 背景：仅纯色深蓝，无星点、无视差。

## 3) 高频调参点
- 动态刷怪参数：Assets/Script/AsteroidSpawner.cs
  - spawnRangeMin / spawnRangeMax
  - targetAsteroidCount
  - minNearbyAsteroids
  - forwardSpawnBias
  - recycleDistance
- 玩家移动参数：Assets/Script/PlayerController.cs
  - moveSpeed
  - thrustForce
  - baseSpeedMultiplier
  - massSpeedGrowth
- 相机体感参数：Assets/Script/CameraFollow.cs
  - smoothSpeed
  - dynamicZoom
  - minDistance / maxDistance

## 4) 低 Token 工作流（建议）
1. 先读本文件，不全量扫项目。
2. 只读取目标文件的局部行段（函数级）。
3. 修改后仅检查受影响文件错误，不跑全项目。
4. 需要定位符号时先全文检索关键词，再读匹配段。
5. 对“状态问答”优先引用本清单，不重复拉全量代码。

## 5) 新对话最小启动模板（可直接粘贴）
请按低 Token 模式处理：
- 先读取 docs/quick-lookup/PROJECT_QUICK_LOOKUP.md
- 只读取与本任务相关的 1-2 个文件
- 不做全量扫描
- 修改后只检查改动文件错误
任务：<在此填你的本次目标>

## 5.1) 新对话模块化启动模板（更省）
请按超低 Token 模式处理：
- 先读取对应模块文件（例如 docs/quick-lookup/QUICK_MODULE_SPAWNING.md）
- 只读取该模块涉及的目标函数片段
- 不做全量扫描
- 修改后只检查改动文件错误
任务：<在此填你的本次目标>

## 6) 维护规则
- 每次大改架构后，只更新本文件对应段落。
- 如果“已落地状态”变化，更新第 2 节日期与内容。
- 若新增核心系统，补到第 1 节和第 3 节。
