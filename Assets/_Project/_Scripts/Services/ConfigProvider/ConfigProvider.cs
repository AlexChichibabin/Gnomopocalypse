using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigProvider : IConfigProvider
{
    private Dictionary<string, LevelConfig> levels;
	private Dictionary<UnitType, UnitConfig> units;
	private LevelConfig[] levelList;
	private UnitConfig[] unitConfigs;
	private ProjectileConfig[] projectileConfigs;
	private SpawnRateConfig spawnRateConfig;
	private AudioConfig audioConfig;

	public int LevelAmount => levelList.Length;
	public UnitConfig[] UnitConfigs => unitConfigs;
	public ProjectileConfig[] ProjectileConfigs => projectileConfigs;
	public SpawnRateConfig SpawnRateConfig => spawnRateConfig;

	public void Load()
	{
		levelList = Resources.LoadAll<LevelConfig>(AssetAddress.LevelsConfigPath);
		unitConfigs = Resources.LoadAll<UnitConfig>(AssetAddress.UnitsConfigPath);
		projectileConfigs = Resources.LoadAll<ProjectileConfig>(AssetAddress.ProjectilesConfigPath);
		spawnRateConfig = Resources.Load<SpawnRateConfig>(AssetAddress.SpawnRateConfigPath);
		audioConfig = Resources.Load<AudioConfig>(AssetAddress.AudioConfigPath);

		levels = levelList.ToDictionary(x => x.SceneName, x => x);
		units = unitConfigs.ToDictionary(x => x.UnitType, x => x);
	}
	public LevelConfig[] GetLevelList() => levelList;
	public LevelConfig GetLevel(int index) => levelList[index];
	public LevelConfig GetLevel(string name) => levels[name];
	public UnitConfig GetUnit(UnitType unitType) => units[unitType];
	public AudioConfig GetAudio() => audioConfig;

	public UnitConfig GetRandomUnitConfig()
	{
		if (unitConfigs == null || unitConfigs.Length == 0)
		{
			Debug.LogError("[ConfigProvider] Unit configs are missing");
			return null;
		}

		float totalWeight = unitConfigs.Sum(x => Mathf.Max(0, x.SpawnProbability));

		if (totalWeight <= 0)
		{
			Debug.LogWarning("[ConfigProvider] Unit spawn probabilities are zero. Returning first unit config");
			return unitConfigs[0];
		}

		float randomValue = Random.Range(0, totalWeight);
		float currentWeight = 0f;

		foreach (UnitConfig unitConfig in unitConfigs)
		{
			currentWeight += Mathf.Max(0, unitConfig.SpawnProbability);

			if (randomValue <= currentWeight)
				return unitConfig;
		}

		return unitConfigs[unitConfigs.Length - 1];
	}

	public ProjectileConfig GetRandomProjectileConfig()
	{
		if (projectileConfigs == null || projectileConfigs.Length == 0)
		{
			Debug.LogError("[ConfigProvider] Projectile configs are missing");
			return null;
		}

		return projectileConfigs[Random.Range(0, projectileConfigs.Length)];
	}

}
