# Visual + Camera 模块速查

范围：背景、相机跟随、缩放体感。

## 相关文件
- Assets/Script/SpaceBackgroundController.cs
- Assets/Script/CameraFollow.cs
- Assets/Script/GameManager.cs

## 当前状态
- 背景当前为单层纯深蓝（无星点、无视差）。
- GameManager 初始化阶段会确保背景脚本挂到主相机。
- CameraFollow 负责平滑跟随与动态缩放。

## 高频参数
- 背景色：SpaceBackgroundController.backgroundColor
- 跟随速度：CameraFollow.smoothSpeed
- 缩放：dynamicZoom / minDistance / maxDistance / zoomSpeed

## 修改建议
- 背景需求变更优先改 SpaceBackgroundController。
- 运动眩晕感优先调 CameraFollow，而不是先动玩家移动公式。
