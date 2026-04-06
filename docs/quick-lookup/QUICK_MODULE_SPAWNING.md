# Spawning 模块速查

范围：小行星生成、回收、密度保障、玩家附近资源可达性。

## 相关文件
- Assets/Script/AsteroidSpawner.cs
- Assets/Script/GameManager.cs

## 当前状态
- 刷怪逻辑已从 GameManager 拆到 AsteroidSpawner。
- 支持：玩家中心生成、前方偏置、附近密度保障、远距回收。

## 高频参数
- spawnRangeMin / spawnRangeMax
- targetAsteroidCount
- minNearbyAsteroids
- forwardSpawnBias
- recycleDistance

## 高频入口
- 初始化：AsteroidSpawner.Initialize
- 维护循环：AsteroidSpawner.Update -> MaintainPopulation
- 位置策略：GetSpawnPositionAroundPlayer
- 回收：RecycleFarAsteroids

## 修改建议
- 只改体验时优先调参数，不先改算法。
- 出现“跑远没资源”优先查 nearby 与 recycle 参数。
