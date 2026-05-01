# Project Agent Notes

## Project Shape

- This is a Unity project.
- Main game code lives under `Assets/_Project/_Scripts`.
- Main scenes live under `Assets/_Project/_Scenes`.
- Project-specific prefabs, resources, and art live under `Assets/_Project/Prefabs`, `Assets/_Project/Resources`, and `Assets/_Project/Art`.
- Zenject is used for dependency injection. Prefer existing installers and project architecture when adding production code.

## Prototyping Area

- `Assets/_Project/Prototyping` is an isolated prototyping sandbox.
- Code, prefabs, scenes, and configs in `Prototyping` are not yet fully part of the main project architecture.
- Treat prototyping code as experimental unless the user explicitly asks to integrate it.
- Do not move prototyping code into the main architecture without discussing the target shape first.
- When editing prototyping files, keep changes local to `Assets/_Project/Prototyping` and any explicitly requested prototype installer/config files.
- When a prototype feature becomes production-ready, it should be migrated deliberately and aligned with existing architecture, naming, config flow, and Zenject bindings.

## Coding Style

- Keep changes small and focused.
- Prefer existing project patterns over introducing new abstractions.
- Use Zenject injection for dependencies that are already bound through installers.
- For Unity `ScriptableObject` configs, use `CreateAssetMenu` when designers need to create assets from the editor.
- Use serialized private fields for inspector-configured data and expose read-only properties when external code needs access.
- Avoid unrelated formatting churn in scene, prefab, and meta files.

## Unity Notes

- Be careful editing `.unity` and `.prefab` YAML manually. Prefer script/code edits unless scene or prefab wiring is explicitly requested.
- If adding a serialized field to a MonoBehaviour, remember that scene or prefab references may still need to be assigned in the Unity editor.
- For pooled MonoBehaviours, prefer Zenject `MonoMemoryPool<T>` and keep spawn/despawn lifecycle methods on the pooled component.
- For frame-based movement, use `Time.deltaTime`; for Rigidbody/Rigidbody2D movement, prefer physics-friendly movement in `FixedUpdate`.

## Verification

- When possible, run a focused compile/build check.
- If Unity package or generated-project errors block command-line verification, report that clearly and distinguish them from errors in changed project code.
