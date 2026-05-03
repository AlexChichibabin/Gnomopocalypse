# Project Agent Notes

## Project Shape

- This is a Unity project.
- Main game code lives under `Assets/_Project/_Scripts`.
- Main scenes live under `Assets/_Project/_Scenes`.
- Project-specific prefabs, resources, and art live under `Assets/_Project/Prefabs`, `Assets/_Project/Resources`, and `Assets/_Project/Art`.
- Zenject is used for dependency injection. Prefer existing installers and project architecture when adding production code.

## Prototyping Area

- `Assets/_Project/Prototyping` is an isolated prototyping sandbox.
- `Assets/_Project/ShootPrototyping` is also an isolated prototyping sandbox for shooting/slingshot experiments.
- Code, prefabs, scenes, and configs in `Prototyping` are not yet fully part of the main project architecture.
- The same is true for `ShootPrototyping`: do not treat it as production architecture unless the user explicitly says so.
- Treat prototyping code as experimental unless the user explicitly asks to integrate it.
- Do not move prototyping code into the main architecture without discussing the target shape first.
- When editing prototyping files, keep changes local to `Assets/_Project/Prototyping`, `Assets/_Project/ShootPrototyping`, and any explicitly requested prototype installer/config files.
- When a prototype feature becomes production-ready, it should be migrated deliberately and aligned with existing architecture, naming, config flow, and Zenject bindings.

## Current Prototype Patterns

- `PrototypeInstaller` is used for prototype-only Zenject bindings.
- Prototype pooled objects use nested `MonoMemoryPool<T>` classes, for example `Unit.UnitPool`, `Tower.TowerPool`, and `Projectile.ProjectilePool`.
- Prototype factories currently spawn pooled objects directly and may be intentionally rough or test-driven.
- `Shooting` is a slingshot-style projectile controller. It exposes `IsMoving`, which becomes true after release.

## Current Gameplay Patterns

- Production gameplay services are bound through `GameInstaller` and `LevelInstaller` using Zenject.
- `GameInstaller` binds global services such as `IConfigProvider`, `IInputService`, `IPlayerProgress`, and `IGameStateMachine`.
- `LevelInstaller` binds level services such as `ILevelStateMachine`, `UnitsFactory`, `ICoroutineRunner`, `UnitsSpawnSettings`, `ShootingAnchor`, `ProjectileSelection`, `IPlayerHealth`, `IGameCondition`, `IUnitTracker`, `IPauseState`, and memory pools for `Unit` and `Projectile`.
- The project has a global flow (`GameStateMachine`) and a per-level flow (`LevelStateMachine`). `GameBootstrapper` applies `GameState.Bootstrap`; `LevelBootstrapper` applies `LevelState.Bootstrap`, which then enters `LevelState.Gameplay`.
- `GameStateMachine` loads `MenuScene` from `BootstrapScene` and loads the next level through `IPlayerProgress.GetNextLevelConfig().SceneName`.
- Current main scenes are `MenuScene`, `GameplayScene`, and `Level2Scene`; scene names are also stored in `Constants`.
- `LevelStateMachine` restores player health, initializes `UnitTracker`, chooses level music by active scene name, unpauses, then enters gameplay. On win/lose it plays SFX, stops music, pauses gameplay, and raises `StateChanged`.
- `GameCondition` drives level end conditions: player death applies `LevelState.Lose`; `UnitTracker.OnAllUnitDeath` applies `LevelState.Win`.
- `UnitTracker` reads `LevelConfig.UnitCountToWin` for the active scene and counts despawned/dead units through `UnitsFactory.DespawnUnit`.
- `IPlayerProgress` currently stores level progress only in memory. `PlayerProgress.AddScore` marks a level complete, and `GetNextLevelConfig` returns the first level with score `0`, falling back to the last level.
- `UnitsFactory` starts spawning only after `ILevelStateMachine` enters `LevelState.Gameplay`.
- `UnitsFactory` uses `SpawnRateConfig` from `Resources/Configs/Units/SpawnRateConfig` and random `UnitConfig` assets from `Resources/Configs/Units`.
- `ConfigProvider.GetRandomUnitConfig` is weighted by `UnitConfig.SpawnProbability`. `UnitConfig` also has `TransformationProbability`, but `ConfigProvider.GetRandomUnitMutationConfig` currently still uses `SpawnProbability`.
- In `SpawnRateStep`, `Minute` means step duration in minutes, `UnitsPerMinute` means spawn frequency during that step, and `PauseUntilNextWave` means the pause in seconds after that step before the next wave. No pause is applied after the last step.
- `UnitsFactory` tracks pause with a local `isPaused` flag from `IPauseState.IsPausedEvent` and manually waits while paused between spawn waits. It does not currently change `Time.timeScale`.
- `UnitsSpawnSettings` provides the spawn center and radius. If `_spawnPoint` is assigned, spawned units use that transform position; otherwise they use the settings object's own position.
- `UnitsFactory` spawns units inside the configured radius, but Y is quantized to 7 fixed positions. X is still random and clamped to stay inside the circle at the selected Y.
- Unit sprite sorting is assigned at spawn from the selected Y lane. Lower Y lanes receive higher `SpriteRenderer.sortingOrder`; current orders are `13` for the highest lane through `19` for the lowest lane.
- `UnitView.SetSortingOrder` applies the order to all four child unit body sprite renderers (`Dirty`, `Smelly`, `Sticky`, `Leaking`) regardless of which body GameObject is currently active.
- If two units overlap and have the same base sorting order, `Unit` resolves the tie in `LateUpdate` with `Physics2D.OverlapCollider`: one unit temporarily gets `sortingOrder + 1`, then returns to its base order when the conflict ends. `Order in Layer` is an integer, so do not use fractional values like `0.5`.
- `Unit` has a life phase coroutine that waits `UnitConfig.MinStayTime`, plays the drink animation, then mutates to a random mutation config. Mutation does not reset current health; health is reset only when the unit is spawned from the pool.
- `Unit` stops movement while drinking, dying, or playing hurt. Drink and death animation completion is reported through `UnitAnimator.AnimationEnded`, with a temporary 3 second timeout fallback in `Unit.PlayAnimationAndWaitForEnd`.
- Projectile hits call `Unit.DealMainDamage()` or `Unit.DealSecondaryDamage()`, not `UnitHealth` directly. `Unit` queues pending damage, plays the `hurt` animation first, waits for `End` or a temporary 2 second timeout fallback, then applies the queued health damage.
- `Unit` does not start drink/mutation if it is already dead (`CurrentHealth <= 0` or `_isDead`). If health reaches zero during a pending mutation, the life phase stops and the death routine takes over.
- `Unit` listens to `IPauseState.IsPaused` and toggles `UnitMove.enabled` while paused.
- Be careful with event cleanup around pooled units: `Unit` currently unsubscribes from `UnitHealth.ZeroHealth` on despawn, but `IPauseState.IsPausedEvent` cleanup should be checked when changing unit lifecycle.
- `UnitAnimator` manages four child `Animator` references for `Smelly`, `Dirty`, `Leaking`, and `Sticky`, plus a PUF animator. All unit animator controllers are expected to have a `Walk` state and `drink`/`death`/`hurt` trigger parameters.
- Unit drink/death/hurt animation clips should call an animation event named `End` at the end of the clip. `UnitAnimationEventReceiver` receives `End()` on the same child GameObject as the `Animator` and forwards it to `UnitAnimator`.
- Drink animation clips can call the animation event function `Puf()` to trigger the C# `Pufed` event. `UnitAnimator` responds by enabling the PUF object and playing its `PUF` state from the start; the PUF object is reset/hidden only on fresh pool spawn, not on mutation.
- Hurt states in unit animator controllers transition back to `Walk` with `Has Exit Time = true`, `Exit Time = 1`, and zero transition duration so short hurt clips can finish without being blended away.
- `ProjectileSelection` manages the current projectile stock as a queue of `ProjectileConfig` assets from `Resources/Configs/Projectiles`. It fills the stock at level start, `ProjectileFactory` takes the bottom/first config when spawning, and `ProjectileSelection` immediately adds a new random config to the top/end.
- `ProjectileSelectionView` displays that stock by rotating slot transforms in `ProjectileSelectionCanvas`, not just swapping sprites. The bottom slot is the next projectile; after taking it, that slot moves to the top under the viewport mask and receives the new reserve config.
- `ProjectileFactory` is not click-driven. It spawns the first projectile on `Start`, initializes its `Shooting`, subscribes to `Shooting.Released`, and spawns the next projectile after serialized `_spawnCooldown` seconds.
- `Projectile.Despawn()` is guarded against double pool returns with `_isDespawned`; reset that flag in `OnSpawned` when changing projectile pooling behavior.
- `Shooting` is controlled through `IInputService.MousePos` plus Unity mouse callbacks (`OnMouseDown`, `OnMouseDrag`, `OnMouseUp`). It refuses drag/release while the projectile is already moving or while `IPauseState.IsPaused` is true.
- `ProjectileTrigger` requires `_shooting.IsMoving` before applying damage. Projectile hits should continue to call methods on `Unit`, not `UnitHealth`, so hurt animation timing stays centralized.
- `WinLosePanel` listens to `LevelStateMachine.StateChanged`, shows win/lose sprites, and uses `IGameStateMachine` to load the next level or `SceneManager.LoadScene` to restart. When touching next-level logic, compare scene names using `LevelConfig.SceneName`, not the asset name.
- `PauseState` disables/enables gameplay input through `IInputService` and raises `IsPausedEvent`; pause UI should avoid showing over win/lose states.
- `AudioService.Init` creates a persistent `[Audio]` GameObject with SFX and music sources, using `AudioConfig` from resources. Avoid creating extra audio roots unless deliberately changing the audio architecture.

## Coding Style

- Keep changes small and focused.
- Prefer existing project patterns over introducing new abstractions.
- Use Zenject injection for dependencies that are already bound through installers.
- For Unity `ScriptableObject` configs, use `CreateAssetMenu` when designers need to create assets from the editor.
- Use serialized private fields for inspector-configured data and expose read-only properties when external code needs access.
- Avoid unrelated formatting churn in scene, prefab, and meta files.
- Prefer English for new code comments and logs.
- Some files may contain Windows-1251 Cyrillic comments. If text looks corrupted, inspect raw bytes or try Windows-1251 before assuming the content is lost.

## Unity Notes

- Be careful editing `.unity` and `.prefab` YAML manually. Prefer script/code edits unless scene or prefab wiring is explicitly requested.
- If adding a serialized field to a MonoBehaviour, remember that scene or prefab references may still need to be assigned in the Unity editor.
- For pooled MonoBehaviours, prefer Zenject `MonoMemoryPool<T>` and keep spawn/despawn lifecycle methods on the pooled component.
- When adding prototype pool bindings, bind them in `PrototypeInstaller` unless the user asks for production integration.
- For frame-based movement, use `Time.deltaTime`; for Rigidbody/Rigidbody2D movement, prefer physics-friendly movement in `FixedUpdate`.
- For `OnPointerClick` on world-space 2D objects, make sure the clicked object has a `Collider2D`, an `EventSystem` exists, and the camera has a `Physics2DRaycaster` whose event mask includes the object's layer.
- For UI clicks, use a `GraphicRaycaster` on the canvas instead of a physics raycaster.
- For `OnTriggerEnter2D`, both objects need `Collider2D`, at least one collider must be a trigger, and at least one object needs `Rigidbody2D`.
- Animation events are resolved on the GameObject that plays the clip. If Unity shows `End (Function Not Supported)`, make sure `UnitAnimationEventReceiver` is present on the same child GameObject as that child `Animator`.
- For 2D Y sorting, do not assume `Transparency Sort Axis` alone will solve unit overlap. It only participates after `Sorting Layer` and `Order in Layer`, and equal/tied renderers may still fall back to creation order. Prefer an explicit per-unit sorting strategy based on a stable feet/pivot point when exact front/back ordering matters.

## Verification

- When possible, run a focused compile/build check.
- If Unity package or generated-project errors block command-line verification, report that clearly and distinguish them from errors in changed project code.
