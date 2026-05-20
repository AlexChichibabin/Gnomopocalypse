using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
public class LevelConfig : ScriptableObject
{
	public string SceneName;
	public int UnitCountToWin;
    public SpawnRateStep[] SpawnRateSteps;

	public ProjectileType[] ProjectileOnLevel;
}


