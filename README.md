<div align="center">

# Kitchen Chaos

**3D Kitchen Simulation Game · Unity / C#**

[![Unity](https://img.shields.io/badge/Unity-2022.3.57f1-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-10.0-512BD4?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![URP](https://img.shields.io/badge/Rendering-URP-blue)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/)

基于 Unity 开发的 3D 厨房模拟经营游戏，采用事件驱动架构 + 数据驱动设计，实现完整的游戏业务闭环。

</div>

---

## Technical Highlights

- **事件驱动架构** — 使用 C# `event EventHandler` 实现模块间解耦，10+ 事件链路覆盖状态变更、交互操作、音效播放等场景，消除跨模块直接依赖
- **ScriptableObject 数据驱动** — 食材属性、配方规则、音效配置等均通过 ScriptableObject 管理，实现策划配置与程序逻辑分离，支持运行时热替换
- **OOP 继承体系** — 设计 `KitchenObjectHolder → BaseCounter → 7 种柜台子类` 的继承链，利用多态统一交互接口 `Interact()/Operate()`，新增柜台类型只需继承并重写
- **有限状态机 (FSM)** — `GameManager` 采用枚举状态机驱动游戏生命周期，`StoveCounter` 实现两阶段烹饪状态机（frying → burning），状态转换触发 UI/音效/视觉反馈
- **New Input System 键位重绑定** — 基于 `PerformInteractiveRebinding` 实现运行时交互式键位配置，通过 `PlayerPrefs` 持久化自定义绑定
- **3D 空间音效** — 事件订阅驱动的全局音效管理，`AudioSource.PlayClipAtPoint` 实现 3D 空间衰减，脚步声音源挂载于相机附近优化听感

---

## Architecture

### 系统架构图

```
┌─────────────────────────────────────────────────────┐
│                    UI Layer                          │
│  CountDownUI · OrderListUI · PlayTimeUI · PauseUI   │
│  SettingUI · CustomKeyUI · DeliveryUI · GameOverUI   │
└──────────────────────┬──────────────────────────────┘
                       │ event subscription
┌──────────────────────▼──────────────────────────────┐
│                 Game Core                            │
│  ┌──────────┐  ┌──────────┐  ┌──────────────────┐   │
│  │GameManager│  │OrderMgr  │  │   GameInput      │   │
│  │  (FSM)   │  │(订单系统) │  │ (InputSystem)   │   │
│  └────┬─────┘  └────┬─────┘  └────────┬─────────┘   │
│       │              │                 │              │
│  ┌────▼──────────────▼─────────────────▼────────┐   │
│  │              Player (Rigidbody + Raycast)     │   │
│  └────┬──────────────────────────────────────────┘   │
│       │ Interact / Operate                           │
│  ┌────▼──────────────────────────────────────────┐   │
│  │  KitchenObjectHolder (基类)                    │   │
│  │      └── BaseCounter (柜台基类)                │   │
│  │           ├── ClearCounter                     │   │
│  │           ├── ContainerCounter                 │   │
│  │           ├── CuttingCounter                   │   │
│  │           ├── StoveCounter (2-phase FSM)       │   │
│  │           ├── PlatesCounter                    │   │
│  │           ├── DeliveryCounter                  │   │
│  │           └── TrashCounter                     │   │
│  └───────────────────────────────────────────────-┘   │
└─────────────────────────────────────────────────────┘
                       │ event subscription
┌──────────────────────▼──────────────────────────────┐
│               Infrastructure                         │
│  SoundManager · MusicManager · DataManager ·         │
│  LoadingManager · WarningControl                    │
└─────────────────────────────────────────────────────┘
```

### 核心设计模式

| 模式 | 应用场景 | 实现方式 |
|------|---------|---------|
| **单例模式** | GameManager / Player / OrderManager 等全局管理器 | 私有构造函数 + `Instance` 静态属性 + `Awake` 初始化保障 |
| **观察者模式** | 游戏状态变更、交互操作、音效触发 | C# `event EventHandler`，发布方触发，订阅方响应 |
| **模板方法** | `BaseCounter.Interact()` 提供通用交互模板 | `virtual` 基类方法 + `override` 子类差异化 |
| **状态模式** | 游戏生命周期、炉灶烹饪阶段 | 枚举 FSM + `switch` 驱动状态转换与副作用 |
| **数据驱动** | 食材/配方/音效等可配置数据 | `ScriptableObject` + `[CreateAssetMenu]` 编辑器创建 |

### 事件驱动解耦

关键事件链路（取代直接方法调用，降低模块间耦合度）：

```csharp
// 发布方：StoveCounter
public static event EventHandler<OnStoveSizzleAegs> OnStoveSizzle;

// 订阅方：StoveSound（无需持有 StoveCounter 引用）
StoveCounter.OnStoveSizzle += (sender, e) => {
    audioSource.Play();  // or Pause()
};
```

| 事件 | 发布者 | 订阅者 | 用途 |
|------|--------|--------|------|
| `OnStateChanged` | GameManager | CountDownUI / PlayTimeUI / GameOverUI | 游戏状态变更驱动 UI 切换 |
| `OnRecipeSpawned` | OrderManager | OrderListUI | 新订单生成刷新列表 |
| `OnRecipeCompleted` | OrderManager | OrderListUI / DeliveryUI / SoundManager | 配送成功多端联动 |
| `OnRecipeFailed` | OrderManager | DeliveryUI / SoundManager | 配送失败反馈 |
| `OnObjectPickedUp` | BaseCounter | SoundManager | 拾取音效 |
| `OnChoping` | CuttingCounter | SoundManager / CuttingAnimator | 切菜音效 + 动画 |
| `OnStoveSizzle` | StoveCounter | StoveSound | 炉灶滋滋声循环 |
| `OnGameToggle` | GameManager | PauseUI | 暂停/恢复菜单 |

---

## Tech Stack

| Category | Technology | Detail |
|----------|-----------|--------|
| Engine | Unity 2022.3.57f1 (LTS) | 长期支持版，稳定可靠 |
| Language | C# (.NET Standard 2.1) | 面向对象 + 事件驱动 |
| Rendering | URP | 通用渲染管线 |
| Input | New Input System | 支持运行时键位重绑定 |
| Data Config | ScriptableObject | 配方/食材/音效数据驱动 |
| Persistence | PlayerPrefs | 设置/键位/首次游玩持久化 |
| UI | UGUI + TextMeshPro | 14 个 UI 面板 |
| Animation | Animator + Animation Clips | 状态机驱动角色/柜台/特效动画 |
| Physics | Raycast + Rigidbody | 射线检测交互 + 物理移动 |

---

## Core Systems

### 1. 游戏状态机 (GameManager)

枚举 FSM 驱动游戏全生命周期，每个状态转换触发 `OnStateChanged` 事件通知 UI 层：

```
TutorialToWaits ──→ waitingToStart ──→ countDownToStart ──→ gamePlaying ──→ overGame
   (1s等待)           (1s延迟)          (3s倒计时)          (150s限时)       (Time.timeScale=0)
```

- `Time.timeScale` 控制暂停/恢复，暂停时禁用输入监听防止误触
- 首次游玩通过 `DataManager` 标记跳过教程

### 2. 玩家控制 (Player + GameInput)

```csharp
// FixedUpdate 中物理移动，避免帧率波动导致位移不一致
rb.MovePosition(rb.position + moveVector * (moveSpeed * Time.fixedDeltaTime));

// Raycast 检测面朝方向的柜台（2m 范围）
Physics.Raycast(transform.position, transform.forward, out hit, 2f, counterLayerMask);

// Slerp 平滑旋转
transform.forward = Vector3.Slerp(transform.forward, moveVector, rotateSpeed * Time.deltaTime);
```

- 移动逻辑放 `FixedUpdate`，交互检测放 `Update`，遵循物理/逻辑分离原则
- `MapBorder` 单例约束边界，`RigidbodyConstraints.FreezeRotation` 防止物理翻转

### 3. 柜台交互体系 (BaseCounter 继承链)

`BaseCounter.Interact()` 实现通用交互模板（玩家/柜台 食材交换 + 盘子组合），子类 `override` 差异化：

| Counter | Interact | Operate | State |
|---------|----------|---------|-------|
| ClearCounter | 基础取放 | — | — |
| ContainerCounter | 取食材 + 打开动画 | 生成食材 | — |
| CuttingCounter | 取放 + 盘子交互 | 多步切割，进度条可视化 | cuttingCount → max |
| StoveCounter | 取放 + 盘子交互 | 开关火 | idle → frying → burning |
| PlatesCounter | 取盘子 + 食材入盘 | — | 定时生成（max 7） |
| DeliveryCounter | 提交盘子匹配订单 | — | — |
| TrashCounter | 销毁食材 | — | — |

### 4. 炉灶两阶段烹饪 (StoveCounter)

```csharp
enum E_CookingState { idle, frying, burning }

// frying → burning 自动转换
if (cookingTime >= fRecipe.cookingTime) {
    DestoryKitchenObject();
    CreatKitchObject(fRecipe.output.prefab);  // 煎炸成品
    cookingState = E_CookingState.burning;    // 进入烧焦倒计时
}

// burning 超 50% 触发警告
if (cookingTime / bRecipe.cookingTime > 0.5f)
    warningControl.StartWarning();  // 视觉变红 + 周期警告音
```

### 5. 订单系统 (OrderManager)

- 随机生成：间隔 8~10s 随机从菜单生成，最大并行 5 单
- 超时机制：每单 45s 倒计时，超时自动移除并触发 `OnRecipeOverTime`
- 匹配判定：`Plate.GetKitchenObjectSoList()` 与 `RecipeSo.kitchenObjectSoList` 逐项比对
- 同一盘禁止重复肉类（`E_Type.meat` 去重校验）

### 6. 键位重绑定 (GameInput + CustomKeyUI)

```csharp
// 交互式重绑定（监听下一次物理按键输入）
iAction.PerformInteractiveRebinding(bindingId)
    .OnComplete(callback => {
        callback.Dispose();
        inputAction.Player.Enable();
        PlayerPrefs.SetString(KEY, inputAction.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
        onFinish?.Invoke();  // 通知 UI 刷新显示
    })
    .Start();
```

### 7. 音频系统

- `SoundManager` 订阅所有游戏事件，统一管理 SFX 播放
- `AudioClip[]` 数组 + `Random.Range` 实现同类型音效随机化（如多种脚步声/切菜声）
- `PlayClipAtPoint` 实现 3D 空间衰减
- 脚步声音源挂载于相机附近（非玩家身上），避免 3D 衰减导致听感不均

---

## Project Structure

```
Assets/Scripts/                   # 52 C# scripts
│
├── Player.cs                     # 玩家：移动 + 交互选择 (Rigidbody + Raycast)
├── GameManager.cs                # 游戏状态机 (FSM) + 暂停恢复
├── GameInput.cs                  # 输入系统 + 交互式键位重绑定
├── OrderManager.cs               # 订单生成/超时/配送判定
├── Plate.cs                      # 盘子食材组合 (SortedList + 类型校验)
├── KitchenObject.cs              # 食材实体基类
├── KitchenObjectHolder.cs        # 持有者基类 (转移/创建/销毁)
├── DataManager.cs                # 设置持久化 (PlayerPrefs + DontDestroyOnLoad)
├── LoadingManager.cs             # 场景异步加载
├── MapBorder.cs                  # 活动边界
├── WarningControl.cs             # 烧焦警告控制器
├── FollowPlayer.cs               # 相机跟随
├── MainmenuManager.cs            # 主菜单
│
├── Counter/                      # 柜台继承体系
│   ├── BaseCounter.cs            #   交互模板 (virtual Interact/Operate)
│   ├── ClearCounter.cs
│   ├── ContainerCounter.cs
│   ├── CuttingCounter.cs         #   多步切割 + 进度条
│   ├── StoveCounter.cs           #   两阶段烹饪 FSM
│   ├── PlatesCounter.cs          #   定时生成盘子
│   ├── DeliveryCounter.cs        #   配送匹配
│   └── TrashCounter.cs
│
├── ScriptObjects/                # ScriptableObject 定义
│   ├── KitchenObjectSo.cs        #   食材数据 (type, priority, prefab, sprite)
│   ├── RecipeSo.cs               #   配方数据
│   ├── CuttingRecipeListSo.cs     #   切菜转换规则
│   ├── CookingRecipeListSo.cs     #   烹饪转换规则
│   └── AudioClipSo.cs             #   音效资源集合
│
├── Animator/                     # 动画控制
│   ├── PlayerAnimator.cs
│   ├── CounterAnimator.cs
│   ├── CuttingAnimator.cs
│   ├── CookingAnimator.cs
│   └── PlateCompleteVisual.cs
│
├── Audio/                        # 音频管理
│   ├── SoundManager.cs           #   全局 SFX (事件驱动)
│   ├── MusicManager.cs           #   BGM
│   ├── StoveSound.cs             #   炉灶滋滋声
│   └── PlayrSound.cs             #   脚步声空间音效
│
└── UI/                           # 14 个 UI 面板
    ├── CountDownUI.cs
    ├── OrderLIstUI.cs
    ├── RecipeUI.cs
    ├── RecipeTimeUI.cs
    ├── DeliveryUI.cs
    ├── PlayTimeUI.cs
    ├── ProgressBarUI.cs
    ├── PauseUI.cs
    ├── SettingUI.cs
    ├── CustomKeyUI.cs
    ├── TutorialUI.cs
    ├── GameOverUI.cs
    ├── KitchenObjectGridUI.cs
    └── LookAtCamera.cs
```

---

## Getting Started

```bash
git clone https://github.com/<your-username>/Kitchen-Chaos.git
```

1. Install [Unity Hub](https://unity.com/download)
2. Add Unity Editor **2022.3.57f1** (LTS)
3. Open the project via Unity Hub → **Add** → select project root
4. Open `Assets/Scenes/MainMenuScene.unity` → **Play**

---

## License

MIT
