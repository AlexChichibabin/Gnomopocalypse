public interface IConfigProvider
{
	int LevelAmount { get; }
	void Load();
	LevelConfig[] GetLevelList();
	LevelConfig GetLevel(int index);
	LevelConfig GetLevel(string name);
	AudioConfig GetAudio();

	UnitConfig[] UnitConfigs { get; }
	ProjectileConfig[] ProjectileConfigs { get; }
	SpawnRateConfig SpawnRateConfig { get; }
	UnitConfig GetRandomUnitConfig();
	UnitConfig GetRandomUnitMutationConfig();
	ProjectileConfig GetRandomProjectileConfig();
	UnitConfig GetUnit(UnitType unitType);

}
