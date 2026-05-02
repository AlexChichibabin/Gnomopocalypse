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
- `ProjectileFactory` is currently click-driven via `IPointerClickHandler`; older coroutine-driven spawn logic may be commented out for testing.
- `Shooting` is a slingshot-style projectile controller. It exposes `IsMoving`, which becomes true after release.

## Current Gameplay Patterns

- Production gameplay services are bound through `GameInstaller` and `LevelInstaller` using Zenject.
- `GameInstaller` binds global services such as `IConfigProvider`, `IInputService`, `IPlayerProgress`, and `IGameStateMachine`.
- `LevelInstaller` binds level services such as `ILevelStateMachine`, `UnitsFactory`, `ICoroutineRunner`, `UnitsSpawnSettings`, `ShootingAnchor`, and memory pools for `Unit` and `Projectile`.
- `UnitsFactory` starts spawning only after `ILevelStateMachine` enters `LevelState.Gameplay`.
- `UnitsFactory` uses `SpawnRateConfig` from `Resources/Configs/Units/SpawnRateConfig` and random `UnitConfig` assets from `Resources/Configs/Units`.
- In `SpawnRateStep`, `Minute` means step duration in minutes, `UnitsPerMinute` means spawn frequency during that step, and `PauseUntilNextWave` means the pause in seconds after that step before the next wave. No pause is applied after the last step; the last step repeats indefinitely.
- `UnitsSpawnSettings` provides the spawn center and radius. If `_spawnPoint` is assigned, spawned units use that transform position; otherwise they use the settings object's own position.
- `ProjectileSelection` manages the current projectile stock as a queue of `ProjectileConfig` assets from `Resources/Configs/Projectiles`. It fills the stock at level start, `ProjectileFactory` takes the bottom/first config when spawning, and `ProjectileSelection` immediately adds a new random config to the top/end.

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

## Verification

- When possible, run a focused compile/build check.
- If Unity package or generated-project errors block command-line verification, report that clearly and distinguish them from errors in changed project code.
