public interface IConfigProvider
{
	int LevelAmount { get; }
	void Load();
	LevelConfig[] GetLevelList();
	LevelConfig GetLevel(int index);
	LevelConfig GetLevel(string name);

	UnitConfig[] UnitConfigs { get; }
	SpawnRateConfig SpawnRateConfig { get; }
	UnitConfig GetRandomUnitConfig();
	UnitConfig GetUnit(UnitType unitType);
}
