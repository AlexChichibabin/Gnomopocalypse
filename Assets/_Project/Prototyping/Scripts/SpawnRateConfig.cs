using UnityEngine;

[CreateAssetMenu(fileName = "SpawnRateConfig", menuName = "Configs/Spawn Rate")]
public class SpawnRateConfig : ScriptableObject
{
    [SerializeField] private SpawnRateStep[] _spawnRateSteps;

    public SpawnRateStep[] SpawnRateSteps => _spawnRateSteps;
}
