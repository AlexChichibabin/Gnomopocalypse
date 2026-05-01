using System.Collections;
using UnityEngine;
using Zenject;

public class UnitsFactory : 
MonoBehaviour                                               //temp
{
    [SerializeField] private Transform _spawnPoint;         //temp
    [SerializeField] private float _spawnRadius = 3;        //temp
    [SerializeField] private float _spawnTime = 2;          //temp

    private Unit.UnitPool _unitPool;

private bool _canSpawn;

    // public UnitsFactory(Unit.UnitPool unitPool)
    // {
    //     _unitPool = unitPool;

    //     //subscribe to the FSM, when the state is game - start spawning
    // }

    [Inject]
    private void Construct(Unit.UnitPool unitPool)          //temp
    {
        _unitPool = unitPool;
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
        while (_canSpawn)
        {
            yield return new WaitForSeconds(_spawnTime);
            SpawnUnit();
        }
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
