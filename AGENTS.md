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
- `SpawnRateConfig` and `SpawnRateStep` describe prototype unit spawn pacing. In `SpawnRateStep`, `Minute` means step duration in minutes, and `UnitsPerMinute` means spawn frequency during that step.
- `ProjectileFactory` is currently click-driven via `IPointerClickHandler`; older coroutine-driven spawn logic may be commented out for testing.
- `Shooting` is a slingshot-style projectile controller. It exposes `IsMoving`, which becomes true after release.

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
- For `OnPointerClick`, make sure the clicked object has an appropriate collider/raycast target, an `EventSystem` exists, and the camera has the needed raycaster for world objects.
- For `OnTriggerEnter2D`, both objects need `Collider2D`, at least one collider must be a trigger, and at least one object needs `Rigidbody2D`.

## Verification

- When possible, run a focused compile/build check.
- If Unity package or generated-project errors block command-line verification, report that clearly and distinguish them from errors in changed project code.
