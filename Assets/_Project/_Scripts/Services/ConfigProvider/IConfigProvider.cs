public interface IConfigProvider
{
	int LevelAmount { get; }
	void Load();
	LevelConfig[] GetLevelList();
	LevelConfig GetLevel(int index);
	LevelConfig GetLevel(string name);
	AudioConfig GetAudio();

	UnitConfig[] UnitConfigs { get; }
	SpawnRateConfig SpawnRateConfig { get; }
	UnitConfig GetRandomUnitConfig();
	UnitConfig GetUnit(UnitType unitType);

}
