using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnRateConfig", menuName = "Configs/Spawn Rate")]
public class SpawnRateConfig : ScriptableObject
{
    [SerializeField] private SpawnRateStep[] _spawnRateSteps;

    public SpawnRateStep[] SpawnRateSteps => _spawnRateSteps;
}

[Serializable]
public class SpawnRateStep 
{
    [SerializeField] private float _minute;
    [SerializeField] private float _unitsPerMinute;
    [SerializeField] private float _pauseUntilNextWave;


    public float Minute => _minute;
    public float UnitsPerMinute => _unitsPerMinute;
    public float PauseUntilNextWave => _pauseUntilNextWave;

}

