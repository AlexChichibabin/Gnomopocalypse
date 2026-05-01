using System.Collections;
using UnityEngine;
using Zenject;

public class UnitsFactory : 
MonoBehaviour                                               //temp
{
    [SerializeField] private Transform _spawnPoint;         //temp
    [SerializeField] private float _spawnRadius = 3;        //temp

    private Unit.UnitPool _unitPool;
    private SpawnRateConfig _spawnRateConfig;
    private bool _canSpawn;

    // public UnitsFactory(Unit.UnitPool unitPool)
    // {
    //     _unitPool = unitPool;

    //     //subscribe to the FSM, when the state is game - start spawning
    // }

    [Inject]
    private void Construct(Unit.UnitPool unitPool, SpawnRateConfig spawnRateConfig)          //temp
    {
        _unitPool = unitPool;
        _spawnRateConfig = spawnRateConfig;
    }

    void OnEnable()                                          //temp
    {
        _canSpawn = true;
        
    }

    void Start()                                            // temp
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()                      //temp
    {
        if (_spawnRateConfig == null)
        {
            Debug.LogWarning("[UnitsFactory] Spawn rate config is missing");
            yield break;
        }

        SpawnRateStep[] spawnRateSteps = _spawnRateConfig.SpawnRateSteps;

        if (spawnRateSteps == null || spawnRateSteps.Length == 0)
        {
            Debug.LogWarning("[UnitsFactory] Spawn rate config is empty");
            yield break;
        }

        for (int i = 0; _canSpawn; i++)
        {
            int stepIndex = Mathf.Min(i, spawnRateSteps.Length - 1);
            SpawnRateStep step = spawnRateSteps[stepIndex];
            bool isLastStep = stepIndex == spawnRateSteps.Length - 1;

            Debug.Log($"[UnitsFactory] Step {stepIndex + 1}: {step.UnitsPerMinute} units/minute");

            yield return SpawnByStep(step, isLastStep);
        }
    }

    private IEnumerator SpawnByStep(SpawnRateStep step, bool isLastStep)
    {
        if (step.UnitsPerMinute <= 0)
        {
            if (isLastStep)
            {
                while (_canSpawn)
                    yield return null;
            }
            else
            {
                yield return new WaitForSeconds(step.Minute * 60f);
            }

            yield break;
        }

        float spawnDelay = 60f / step.UnitsPerMinute;

        if (isLastStep)
        {
            while (_canSpawn)
            {
                yield return new WaitForSeconds(spawnDelay);
                SpawnUnit();
            }

            yield break;
        }

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
        Unit unit = _unitPool.Spawn();
        unit.transform.position = GetRandomSpawnPosition();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 offset = Random.insideUnitCircle * _spawnRadius;

        return _spawnPoint.position + new Vector3(offset.x, offset.y, 0);
    }

    void OnDisable()                                        //temp
    {
        _canSpawn = false;
    }
}
