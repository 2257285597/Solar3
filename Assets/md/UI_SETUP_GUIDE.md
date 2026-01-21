# Unity UI 完整配置指南

## ?? 目录

1. [创建 UIManager 对象](#1-创建-uimanager-对象)
2. [创建游戏信息 UI](#2-创建游戏信息-ui)
3. [创建分支选择界面](#3-创建分支选择界面)
4. [创建突变选择界面](#4-创建突变选择界面)
5. [创建操作提示界面](#5-创建操作提示界面)
6. [连接引用](#6-连接引用)
7. [测试 UI](#7-测试-ui)

---

## 1. 创建 UIManager 对象

### 步骤 1.1：创建空物体

```
Hierarchy 窗口 → 右键 → Create Empty
重命名为：UIManager
```

### 步骤 1.2：添加脚本

```
选中 UIManager → Inspector → Add Component
搜索并添加：UIManager 脚本
```

---

## 2. 创建游戏信息 UI

### 步骤 2.1：创建 Canvas

```
Hierarchy → 右键 → UI → Canvas

Canvas 设置：
├─ Render Mode: Screen Space - Overlay
├─ UI Scale Mode: Scale With Screen Size
└─ Reference Resolution: 1920 x 1080
```

### 步骤 2.2：创建游戏信息面板

在 Canvas 下创建：

```
Canvas
└─ GameInfoPanel (Panel)
   ├─ MassText (Text)
   ├─ StageText (Text)
   ├─ EvolutionSlider (Slider)
   ├─ VelocityText (Text)
   └─ FPSText (Text)
```

#### 详细创建步骤：

**A. 创建 GameInfoPanel**

```
Canvas → 右键 → UI → Panel
重命名：GameInfoPanel

设置：
├─ Rect Transform:
│  ├─ Anchor: Top Left
│  ├─ Position X: 150
│  ├─ Position Y: -20
│  ├─ Width: 300
│  └─ Height: 200
└─ Image:
   ├─ Color: 半透明黑色 (R:0, G:0, B:0, A:150)
   └─ Material: None (Default)
```

**B. 创建 MassText**

```
GameInfoPanel → 右键 → UI → Text
重命名：MassText

设置：
├─ Rect Transform:
│  ├─ Anchor: Top Left
│  ├─ Position X: 10
│  ├─ Position Y: -10
│  ├─ Width: 280
│  └─ Height: 30
└─ Text:
   ├─ Text: "质量: 0 / 10"
   ├─ Font: Arial
   ├─ Font Size: 18
   ├─ Color: 白色
   ├─ Alignment: Left, Top
   └─ Best Fit: 勾选（可选）
```

**C. 创建 StageText**

```
GameInfoPanel → 右键 → UI → Text
重命名：StageText

设置：
├─ Rect Transform:
│  ├─ Anchor: Top Left
│  ├─ Position X: 10
│  ├─ Position Y: -45
│  ├─ Width: 280
│  └─ Height: 30
└─ Text:
   ├─ Text: "阶段: 陨石"
   ├─ Font Size: 18
   ├─ Color: 黄色 (R:255, G:255, B:0)
   └─ Alignment: Left, Top
```

**D. 创建 EvolutionSlider**

```
GameInfoPanel → 右键 → UI → Slider
重命名：EvolutionSlider

设置：
├─ Rect Transform:
│  ├─ Anchor: Top Left
│  ├─ Position X: 150
│  ├─ Position Y: -90
│  ├─ Width: 280
│  └─ Height: 20
└─ Slider:
   ├─ Min Value: 0
   ├─ Max Value: 1
   ├─ Value: 0
   └─ Whole Numbers: 取消勾选

子元素设置：
├─ Background: 深灰色
├─ Fill: 绿色 (R:0, G:255, B:0)
└─ Handle: 隐藏（删除或禁用）
```

**E. 创建 VelocityText**

```
GameInfoPanel → 右键 → UI → Text
重命名：VelocityText

设置：
├─ Position X: 10
├─ Position Y: -115
├─ Width: 280
├─ Height: 30
└─ Text: "速度: 0"
   ├─ Font Size: 16
   └─ Color: 青色 (R:0, G:255, B:255)
```

**F. 创建 FPSText**

```
GameInfoPanel → 右键 → UI → Text
重命名：FPSText

设置：
├─ Position X: 10
├─ Position Y: -145
├─ Width: 280
├─ Height: 30
└─ Text: "FPS: 60"
   ├─ Font Size: 14
   └─ Color: 灰色
```

---

## 3. 创建分支选择界面

### 步骤 3.1：创建分支选择面板

```
Canvas → 右键 → UI → Panel
重命名：BranchSelectionPanel

设置：
├─ Rect Transform:
│  ├─ Anchor: Center
│  ├─ Position: (0, 0)
│  ├─ Width: 800
│  └─ Height: 500
└─ Image:
   ├─ Color: 半透明黑色 (A:200)
   └─ 勾选 Raycast Target（阻挡点击穿透）
```

### 步骤 3.2：创建标题

```
BranchSelectionPanel → 右键 → UI → Text
重命名：TitleText

设置：
├─ Rect Transform:
│  ├─ Anchor: Top Center
│  ├─ Position Y: -30
│  ├─ Width: 600
│  └─ Height: 50
└─ Text:
   ├─ Text: "选择行星进化分支"
   ├─ Font Size: 32
   ├─ Font Style: Bold
   ├─ Color: 金色 (R:255, G:215, B:0)
   └─ Alignment: Center
```

### 步骤 3.3：创建三个分支按钮

#### 按钮 1：冰封堡垒

```
BranchSelectionPanel → 右键 → UI → Button
重命名：IceFortressButton

设置：
├─ Rect Transform:
│  ├─ Anchor: Center
│  ├─ Position: (-250, 0)
│  ├─ Width: 200
│  └─ Height: 300
└─ Image:
   ├─ Color: 冰蓝色 (R:150, G:200, B:255)
   └─ Sprite: UI/Skin/UISprite (默认)

子元素 Text 设置：
├─ Text: "冰封堡垒\n\n高防御\n冰冻效果"
├─ Font Size: 18
├─ Color: 白色
└─ Alignment: Center
```

#### 按钮 2：生命摇篮

```
BranchSelectionPanel → 右键 → UI → Button
重命名：LifeCradleButton

设置：
├─ Position: (0, 0)
├─ Width: 200
├─ Height: 300
└─ Color: 生命绿 (R:100, G:255, B:100)

子元素 Text：
└─ Text: "生命摇篮\n\n高回复\n生命辅助"
```

#### 按钮 3：战争星球

```
BranchSelectionPanel → 右键 → UI → Button
重命名：WarPlanetButton

设置：
├─ Position: (250, 0)
├─ Width: 200
├─ Height: 300
└─ Color: 战争红 (R:255, G:100, B:100)

子元素 Text：
└─ Text: "战争星球\n\n高攻击\n武器系统"
```

### 步骤 3.4：隐藏分支面板

```
选中 BranchSelectionPanel
Inspector → 取消勾选左上角复选框（禁用）
```

---

## 4. 创建突变选择界面

### 步骤 4.1：创建突变选择面板

```
Canvas → 右键 → UI → Panel
重命名：MutationSelectionPanel

设置：
├─ Rect Transform:
│  ├─ Anchor: Center
│  ├─ Position: (0, 0)
│  ├─ Width: 900
│  └─ Height: 400
└─ Image:
   └─ Color: 半透明黑色 (A:220)
```

### 步骤 4.2：创建标题

```
MutationSelectionPanel → 右键 → UI → Text
重命名：MutationTitleText

设置：
├─ Position Y: -30
├─ Width: 600
├─ Height: 50
└─ Text: "选择突变强化（3选1）"
   ├─ Font Size: 28
   ├─ Font Style: Bold
   └─ Color: 紫色 (R:200, G:100, B:255)
```

### 步骤 4.3：创建三个突变按钮

#### 突变按钮 1

```
MutationSelectionPanel → 右键 → UI → Button
重命名：MutationButton1

设置：
├─ Position: (-280, 0)
├─ Width: 250
├─ Height: 250
└─ Image Color: 深灰色 (R:80, G:80, B:100)

删除默认的 Text 子元素，重新创建：

MutationButton1 → 右键 → UI → Text
重命名：MutationText1

设置：
├─ Rect Transform: Stretch（拉伸填满按钮）
├─ Text: "突变名称\n\n效果描述"
├─ Font Size: 16
├─ Color: 白色
├─ Alignment: Center
└─ Vertical Overflow: Wrap
```

#### 突变按钮 2 和 3

```
同样的方法创建 MutationButton2 和 MutationButton3

MutationButton2:
└─ Position: (0, 0)

MutationButton3:
└─ Position: (280, 0)
```

### 步骤 4.4：隐藏突变面板

```
选中 MutationSelectionPanel
Inspector → 取消勾选左上角复选框（禁用）
```

---

## 5. 创建操作提示界面

### 步骤 5.1：创建操作提示面板

```
Canvas → 右键 → UI → Panel
重命名：ControlHintPanel

设置：
├─ Rect Transform:
│  ├─ Anchor: Bottom Right
│  ├─ Position X: -10
│  ├─ Position Y: 10
│  ├─ Width: 250
│  └─ Height: 150
└─ Image:
   └─ Color: 半透明黑色 (A:100)
```

### 步骤 5.2：创建提示文本

```
ControlHintPanel → 右键 → UI → Text
重命名：ControlHintText

设置：
├─ Rect Transform: Stretch（填满面板）
├─ Padding: Left:10, Top:10, Right:10, Bottom:10
└─ Text:
   ├─ Text: "操作说明：..."（会由代码自动设置）
   ├─ Font Size: 14
   ├─ Color: 白色
   ├─ Alignment: Left, Top
   └─ Best Fit: 勾选
```

---

## 6. 连接引用

### 步骤 6.1：选中 UIManager

```
Hierarchy → 选中 UIManager
```

### 步骤 6.2：拖拽引用

在 Inspector 的 UIManager 组件中，按顺序拖拽：

#### ▼ 游戏信息显示

```
Mass Text:       拖入 MassText
Stage Text:      拖入 StageText
Evolution Slider: 拖入 EvolutionSlider
Velocity Text:   拖入 VelocityText
FPS Text:        拖入 FPSText
```

#### ▼ 分支选择界面

```
Branch Selection Panel:  拖入 BranchSelectionPanel
Ice Fortress Button:     拖入 IceFortressButton
Life Cradle Button:      拖入 LifeCradleButton
War Planet Button:       拖入 WarPlanetButton
```

#### ▼ 突变选择界面

```
Mutation Selection Panel: 拖入 MutationSelectionPanel
Mutation Button1:         拖入 MutationButton1
Mutation Button2:         拖入 MutationButton2
Mutation Button3:         拖入 MutationButton3
Mutation Text1:           拖入 MutationText1
Mutation Text2:           拖入 MutationText2
Mutation Text3:           拖入 MutationText3
```

#### ▼ 操作提示

```
Control Hint Panel: 拖入 ControlHintPanel
Control Hint Text:  拖入 ControlHintText
```

### 步骤 6.3：检查引用

确保所有字段都已填充，没有 `None (Text)` 或 `None (GameObject)`。

---

## 7. 测试 UI

### 步骤 7.1：运行游戏

```
按 Play 键
```

### 步骤 7.2：检查显示

**应该看到：**
- ? 左上角显示游戏信息面板
- ? 质量显示为 "质量: 1.0 / 10"
- ? 阶段显示为 "阶段: 陨石"
- ? 进化条为空（0%）
- ? 右下角显示操作提示
- ? FPS 正常显示（60左右）

**不应该看到：**
- ? 分支选择面板（应该隐藏）
- ? 突变选择面板（应该隐藏）

### 步骤 7.3：测试进化

```
1. 吞噬小行星增加质量
2. 观察进化条增长
3. 质量达到 10 时，应该触发进化
4. 不会显示分支选择（因为还没到行星阶段）
```

### 步骤 7.4：测试分支选择

```
方法 1：调整进化阈值快速测试
└─ Player 预制体 → Evolution To Planet 改为 10
   (这样进化一次就到行星)

方法 2：使用 Debug 代码
└─ 在 Console 输入：
   GameManager.Instance.ShowPlanetBranchSelection(player);
```

---

## ?? UI 层级结构总览

```
Canvas
├─ GameInfoPanel (显示)
│  ├─ MassText
│  ├─ StageText
│  ├─ EvolutionSlider
│  │  ├─ Background
│  │  ├─ Fill Area
│  │  │  └─ Fill
│  │  └─ Handle Slide Area (可删除)
│  ├─ VelocityText
│  └─ FPSText
│
├─ BranchSelectionPanel (隐藏)
│  ├─ TitleText
│  ├─ IceFortressButton
│  │  └─ Text
│  ├─ LifeCradleButton
│  │  └─ Text
│  └─ WarPlanetButton
│     └─ Text
│
├─ MutationSelectionPanel (隐藏)
│  ├─ MutationTitleText
│  ├─ MutationButton1
│  │  └─ MutationText1
│  ├─ MutationButton2
│  │  └─ MutationText2
│  └─ MutationButton3
│     └─ MutationText3
│
└─ ControlHintPanel (显示)
   └─ ControlHintText
```

---

## ?? UI 样式推荐配置

### 颜色方案

```
主要颜色：
├─ 背景面板：黑色半透明 (0, 0, 0, 150-220)
├─ 主文字：白色 (255, 255, 255)
├─ 强调文字：金色 (255, 215, 0)
└─ 按钮边框：深灰 (80, 80, 100)

分支颜色：
├─ 冰封堡垒：冰蓝 (150, 200, 255)
├─ 生命摇篮：生命绿 (100, 255, 100)
└─ 战争星球：战争红 (255, 100, 100)

进度条：
├─ 背景：深灰 (60, 60, 60)
└─ 填充：绿色渐变 (0, 255, 0)
```

### 字体大小

```
特大标题：32-36
大标题：24-28
正文：16-18
小字：12-14
```

---

## ?? 常见问题

### Q1: UI 在 Game 窗口看不到？

**A**: 检查 Canvas 设置
```
Canvas → Render Mode: Screen Space - Overlay
Canvas → Sort Order: 0 或更高
```

### Q2: 文字显示不全/被裁剪？

**A**: 调整 Rect Transform
```
选中 Text → Rect Transform
增加 Width 和 Height
```

### Q3: 按钮点击没反应？

**A**: 检查事件系统
```
Hierarchy 中应该有 EventSystem
如果没有：GameObject → UI → Event System
```

### Q4: Slider 拖不动/没有 Handle？

**A**: Slider 用于显示进度，不需要 Handle
```
EvolutionSlider → 删除 Handle Slide Area
Slider 组件 → Interactable: 取消勾选
```

### Q5: 面板无法隐藏/显示？

**A**: 检查代码引用
```
UIManager → Inspector
确保所有 Panel 引用已正确拖入
```

---

## ? 配置检查清单

### 基础 UI

- [ ] Canvas 已创建，Render Mode 正确
- [ ] EventSystem 存在
- [ ] UIManager 脚本已添加

### 游戏信息面板

- [ ] GameInfoPanel 已创建并可见
- [ ] MassText 已创建并引用
- [ ] StageText 已创建并引用
- [ ] EvolutionSlider 已创建并引用
- [ ] VelocityText 已创建并引用
- [ ] FPSText 已创建并引用

### 分支选择面板

- [ ] BranchSelectionPanel 已创建并隐藏
- [ ] 三个分支按钮已创建
- [ ] 按钮文字正确
- [ ] 引用已拖入 UIManager

### 突变选择面板

- [ ] MutationSelectionPanel 已创建并隐藏
- [ ] 三个突变按钮已创建
- [ ] 三个突变文本已创建
- [ ] 引用已拖入 UIManager

### 操作提示

- [ ] ControlHintPanel 已创建并可见
- [ ] ControlHintText 已创建并引用

### 测试

- [ ] 运行游戏能看到游戏信息
- [ ] FPS 正常显示
- [ ] 吞噬小行星后质量增加
- [ ] 进化条正常增长

---

## ?? 下一步

UI 配置完成后：

1. **测试所有 UI 元素**
   - 游戏信息实时更新
   - 分支选择正常弹出
   - 突变选择正常工作

2. **优化 UI 样式**
   - 调整颜色方案
   - 添加背景图
   - 添加按钮动画

3. **添加音效**
   - 按钮点击音效
   - UI 弹出音效
   - 进化提示音

4. **添加粒子特效**
   - 进化时的光效
   - 按钮悬停特效

---

完整配置后的效果预览：

```
┌─────────────────────────────────────────┐
│ [游戏信息]                               │
│ 质量: 15.5 / 50                          │
│ 阶段: 小行星                             │
│ [=====>          ] 31%                   │
│ 速度: 3.2        FPS: 60                 │
│                                          │
│                                          │
│          [游戏区域]                      │
│                                          │
│                                          │
│                                          │
│                          [操作说明]      │
│                          WASD - 移动      │
│                          空格 - 冲刺      │
│                          Shift - 引力     │
└─────────────────────────────────────────┘
```

现在开始配置你的 UI 吧！??
