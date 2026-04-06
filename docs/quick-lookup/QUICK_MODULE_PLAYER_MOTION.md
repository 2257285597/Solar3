# Player Motion 模块速查

范围：移动、速度上限、冲刺、技能入口。

## 相关文件
- Assets/Script/PlayerController.cs
- Assets/Script/CameraFollow.cs

## 当前状态
- 质量越大速度越快（正向增长）。
- 使用 ForceMode.Acceleration，避免质量增大导致被动减速体感。
- 分支主动技能函数存在，但主要仍是 TODO 占位。

## 高频参数
- moveSpeed
- thrustForce
- baseSpeedMultiplier
- massSpeedGrowth
- dashSpeed / dashCooldown

## 高频入口
- 移动：HandleMovement
- 冲刺：TryDash
- 技能入口：UseAbility

## 修改建议
- 先调参数再改公式。
- 若体感“慢/晕”，联动检查 CameraFollow 的 smoothSpeed 与 zoom 参数。
