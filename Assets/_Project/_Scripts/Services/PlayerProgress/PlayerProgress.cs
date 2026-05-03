using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProgress : IPlayerProgress
{
	private IConfigProvider configProvider;

	private LevelConfig[] levelList;
	private Dictionary<LevelConfig, int> levelProgress;
	public PlayerProgress(IConfigProvider configProvider)
	{
		this.configProvider = configProvider;
	}
	public void Init() // Не забыть
	{
		levelList = configProvider.GetLevelList();

		levelProgress = new Dictionary<LevelConfig, int>();

		levelProgress = levelList.ToDictionary(x => x, x => 0);
	}
	public LevelConfig GetNextLevelConfig()
	{
		for (int i = 0; i < levelProgress.Count; i++)
		{
			LevelConfig config = levelList[i];

			if (levelProgress[config] != 0) continue;
			return config;
		}
		return levelList[levelList.Length - 1];
	}

	public void AddScore(LevelConfig config, int score)
	{
		levelProgress[config] = 3;
	}
}
