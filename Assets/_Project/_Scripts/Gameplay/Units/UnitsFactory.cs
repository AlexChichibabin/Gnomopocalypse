using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class UnitsFactory : IInitializable, ILateDisposable
{
    private const int SpawnYPositionsCount = 7;
    private const int HighestYSortingOrder = 13;
    private const int LowestYSortingOrder = HighestYSortingOrder + SpawnYPositionsCount - 1;

    private Unit.UnitPool _unitPool;
    private IConfigProvider _configProvider;
    private ICoroutineRunner _coroutineRunner;
    private UnitsSpawnSettings _spawnSettings;
    private Coroutine _spawnRoutine;
    private bool _canSpawn;
    private ILevelStateMachine _levelStateMachine;
    private IUnitTracker _unitTracker;
    private IPauseState _pauseState;
    private bool isPaused = false;

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
		this.isPaused = isPaused;
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

			while (isPaused) yield return null;
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
		while (isPaused) yield return null;
		if (step.PauseUntilNextWave <= 0)
            yield break;

        Debug.Log($"[UnitsFactory] Pause before next wave: {step.PauseUntilNextWave} seconds");
        yield return new WaitForSeconds(step.PauseUntilNextWave);
    }

    private IEnumerator SpawnByStep(SpawnRateStep step)
    {
        if (step.UnitsPerMinute <= 0)
        {
            while (isPaused) yield return null;
            yield return new WaitForSeconds(step.Minute * 60f);
            yield break;
        }

        float spawnDelay = 60f / step.UnitsPerMinute;
        float spawnCount = step.Minute * step.UnitsPerMinute;
        float elapsedTime = 0f;

        for (int i = 0; _canSpawn && i < spawnCount; i++)
        {
			while (isPaused) yield return null;
			yield return new WaitForSeconds(spawnDelay);
            elapsedTime += spawnDelay;
			while (isPaused) yield return null;
			SpawnUnit();
        }

        float stepDuration = step.Minute * 60f;

		while (isPaused) yield return null;
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
        SpawnPosition spawnPosition = GetRandomSpawnPosition();
        unit.transform.position = spawnPosition.Position;
        unit.SetSortingOrder(spawnPosition.SortingOrder);
    }
	public void DespawnUnit(Unit unit)
	{
        _unitPool.Despawn(unit);
        _unitTracker.AddUnitDeath();
	}
	private SpawnPosition GetRandomSpawnPosition()
    {
        float spawnRadius = _spawnSettings.SpawnRadius;
        int yPositionIndex = UnityEngine.Random.Range(0, SpawnYPositionsCount);
        float normalizedYPosition = yPositionIndex / (float)(SpawnYPositionsCount - 1);
        float yOffset = Mathf.Lerp(-spawnRadius, spawnRadius, normalizedYPosition);
        float maxXOffset = Mathf.Sqrt(spawnRadius * spawnRadius - yOffset * yOffset);
        float xOffset = UnityEngine.Random.Range(-maxXOffset, maxXOffset);
        int sortingOrder = LowestYSortingOrder - yPositionIndex;

        return new SpawnPosition(
            _spawnSettings.SpawnPoint + new Vector3(xOffset, yOffset, 0),
            sortingOrder);
    }

    private readonly struct SpawnPosition
    {
        public SpawnPosition(Vector3 position, int sortingOrder)
        {
            Position = position;
            SortingOrder = sortingOrder;
        }

        public Vector3 Position { get; }
        public int SortingOrder { get; }
    }

    public void LateDispose()
    {
        _canSpawn = false;
        _levelStateMachine.StateChanged -= OnStateChanged;
        _coroutineRunner?.Stop(_spawnRoutine);
        _spawnRoutine = null;
    }

}
