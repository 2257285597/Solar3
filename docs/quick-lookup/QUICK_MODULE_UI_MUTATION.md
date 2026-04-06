# UI + Mutation 模块速查

范围：分支选择 UI、突变选择 UI、突变数据与应用。

## 相关文件
- Assets/Script/UIManager.cs
- Assets/Script/Mutation.cs
- Assets/Script/MutationDatabase.cs
- Assets/Script/GameManager.cs

## 当前状态
- UI 选择链路可用：显示 -> 点击 -> 应用。
- 突变数据库有数据，随机抽取可用。
- Mutation.ApplyTo 仅部分字段生效（质量、引力），其余效果待实现。

## 高频入口
- 分支 UI：UIManager.ShowBranchSelection / SelectBranch
- 突变 UI：UIManager.ShowMutationSelection / SelectMutation
- 应用逻辑：Mutation.ApplyTo
- 触发桥接：GameManager.ShowMutationSelection

## 修改建议
- 新增突变效果优先改 Mutation.ApplyTo。
- UI 文案或按钮逻辑只改 UIManager 对应函数，避免跨文件扩散。
