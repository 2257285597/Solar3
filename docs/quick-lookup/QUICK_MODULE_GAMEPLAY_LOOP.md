# Gameplay Loop 模块速查

范围：进化、分支、吞噬、规则判断。

## 相关文件
- Assets/Script/CelestialBody.cs
- Assets/Script/GameManager.cs

## 当前状态
- 吞噬与进化链路可用。
- 进化后会触发分支或突变选择。
- 分支被动加成已落地（部分数值型）。

## 高频入口
- 进化触发：CelestialBody.TriggerEvolution
- 分支设置：CelestialBody.SetPlanetBranch / ApplyBranchBonus
- UI桥接：GameManager.ShowPlanetBranchSelection / ShowMutationSelection

## 修改建议
- 改规则优先读 CelestialBody.cs 对应函数，不先扫全文件。
- 改流程跳转优先读 GameManager.cs 的 UI 桥接函数。
