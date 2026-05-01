public interface IConfigProvider
{
	int LevelAmount { get; }
	void Load();
	LevelConfig[] GetLevelList();
	LevelConfig GetLevel(int index);
	LevelConfig GetLevel(string name);
}