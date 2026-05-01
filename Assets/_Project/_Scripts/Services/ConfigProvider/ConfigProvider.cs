using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigProvider : IConfigProvider
{
    private Dictionary<string, LevelConfig> levels;
	private LevelConfig[] levelList;

	public int LevelAmount => levelList.Length;

	public void Load()
	{
		levelList = Resources.LoadAll<LevelConfig>(AssetAddress.LevelsConfigPath);

		levels = levelList.ToDictionary(x => x.SceneName, x => x);
	}
	public LevelConfig[] GetLevelList() => levelList;
	public LevelConfig GetLevel(int index) => levelList[index];
	public LevelConfig GetLevel(string name) => levels[name];

}