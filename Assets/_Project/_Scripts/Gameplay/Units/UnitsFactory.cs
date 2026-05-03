using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class UnitsFactory : IInitializable, ILateDisposable
{
    private Unit.UnitPool _unitPool;
    private IConfigProvider _configProvider;
    private ICoroutineRunner _coroutineRunner;
    private UnitsSpawnSettings _spawnSettings;
    private Coroutine _spawnRoutine;
    private bool _canSpawn;
    private ILevelStateMachine _levelStateMachine;
    private IUnitTracker _unitTracker;
    private IPauseState _pauseState;

    public event Action OnAllWavesFinished;

    public UnitsFactory(Unit.UnitPool unitPool,
            IConfigProvider configProvider,
            ICoroutineRunner coroutineRunner,
            ILevelStateMachine levelStateMachine,
            UnitsSpawnSettings spawnSettings,
			IUnitTracker unitTracker,
			IPauseState pauseState)
    {
        _unitPool = unitPool;
        _configProvider = configProvider;
        _coroutineRunner = coroutineRunner;
        _levelStateMachine = levelStateMachine;
        _spawnSettings = spawnSettings;
        _unitTracker = unitTracker;
		_pauseState = pauseState;
	}

    public void Initialize()
    {
        _canSpawn = true;
        _levelStateMachine.StateChanged += OnStateChanged;
        _pauseState.IsPausedEvent += OnPausedChanged;
    }

	private void OnPausedChanged(bool isPaused)
	{
		_coroutineRunner.SetActive(!isPaused);
	}

	private void OnStateChanged(LevelState state)
    {
        if (state != LevelState.Gameplay)
            return;

        _levelStateMachine.StateChanged -= OnStateChanged;

        if (_spawnRoutine == null)
            _spawnRoutine = _coroutineRunner.Run(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()                      
    {
        SpawnRateConfig spawnRateConfig = _configProvider.SpawnRateConfig;

        if (spawnRateConfig == null)
        {
            Debug.LogWarning("[UnitsFactory] Spawn rate config is missing");
            yield break;
        }

        SpawnRateStep[] spawnRateSteps = spawnRateConfig.SpawnRateSteps;

        if (spawnRateSteps == null || spawnRateSteps.Length == 0)
        {
            Debug.LogWarning("[UnitsFactory] Spawn rate config is empty");
            yield break;
        }

        for (int i = 0; _canSpawn && i < spawnRateSteps.Length; i++)
        {
            SpawnRateStep step = spawnRateSteps[i];
            bool isLastStep = i == spawnRateSteps.Length - 1;

            Debug.Log($"[UnitsFactory] Step {i + 1}: {step.UnitsPerMinute} units/minute");

            yield return SpawnByStep(step);

            if (_canSpawn && !isLastStep)
                yield return WaitForNextWavePause(step);
        }

        _spawnRoutine = null;

        if (_canSpawn)
            OnAllWavesFinished?.Invoke();
    }

    private IEnumerator WaitForNextWavePause(SpawnRateStep step)
    {
        if (step.PauseUntilNextWave <= 0)
            yield break;

        Debug.Log($"[UnitsFactory] Pause before next wave: {step.PauseUntilNextWave} seconds");
        yield return new WaitForSeconds(step.PauseUntilNextWave);
    }

    private IEnumerator SpawnByStep(SpawnRateStep step)
    {
        if (step.UnitsPerMinute <= 0)
        {
            yield return new WaitForSeconds(step.Minute * 60f);
            yield break;
        }

        float spawnDelay = 60f / step.UnitsPerMinute;
        float spawnCount = step.Minute * step.UnitsPerMinute;
        float elapsedTime = 0f;

        for (int i = 0; _canSpawn && i < spawnCount; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            elapsedTime += spawnDelay;
            SpawnUnit();
        }

        float stepDuration = step.Minute * 60f;

        if (_canSpawn && elapsedTime < stepDuration)
            yield return new WaitForSeconds(stepDuration - elapsedTime);
    }

    public void SpawnUnit()
    {
        UnitConfig unitConfig = _configProvider.GetRandomUnitConfig();

        if (unitConfig == null)
            return;

        Unit unit = _unitPool.Spawn(unitConfig);
        unit.SetPool(this);
        unit.transform.position = GetRandomSpawnPosition();
    }
	public void DespawnUnit(Unit unit)
	{
        _unitPool.Despawn(unit);
        _unitTracker.AddUnitDeath();
	}
	private Vector3 GetRandomSpawnPosition()
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * _spawnSettings.SpawnRadius;

        return _spawnSettings.SpawnPoint + new Vector3(offset.x, offset.y, 0);
    }

    public void LateDispose()
    {
        _canSpawn = false;
        _levelStateMachine.StateChanged -= OnStateChanged;
        _coroutineRunner?.Stop(_spawnRoutine);
        _spawnRoutine = null;
    }

}
